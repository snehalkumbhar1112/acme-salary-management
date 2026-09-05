# ACME Salary Management — Architecture

## 1. Architecture Style

The application uses a modular monolith architecture.

The frontend is an Angular 22 application communicating with an ASP.NET Core
.NET 8 Web API over HTTP/JSON.

The backend uses Entity Framework Core with SQLite.

A modular monolith is intentionally chosen instead of microservices because the
MVP has a focused domain, a single primary user persona, and a dataset of
approximately 10,000 employees. This keeps deployment, testing, and maintenance
simple while leaving room for future modularization.

## 2. Technology Stack

- Frontend: Angular 22
- Backend: ASP.NET Core Web API (.NET 8)
- ORM: Entity Framework Core
- Database: SQLite
- API documentation: Swagger/OpenAPI
- Backend tests: xUnit
- Frontend tests: Angular testing framework
- Version control: Git/GitHub

## 3. Application Flow

Angular UI
    ↓
HTTP/JSON
    ↓
ASP.NET Core Controllers
    ↓
Application Services
    ↓
Entity Framework Core
    ↓
SQLite

Controllers are responsible for HTTP concerns.
Services contain business rules.
EF Core handles persistence.

DTOs are used at the API boundary instead of exposing database entities
directly.

## 4. Core Data Model

### Employee

Stores employee identity and organizational information.

Fields:

- Id
- EmployeeCode
- FirstName
- LastName
- Email
- CountryId
- DepartmentId
- JobTitle
- EmploymentStatus
- HireDate
- CreatedAt
- UpdatedAt

### Country

Stores country and default currency information.

Fields:

- Id
- Name
- Code
- CurrencyCode

### Department

Stores organizational departments.

Fields:

- Id
- Name

### Salary

Stores effective-dated compensation records.

Fields:

- Id
- EmployeeId
- BaseSalary
- Bonus
- CurrencyCode
- EffectiveFrom
- EffectiveTo
- ChangeReason
- CreatedAt

Salary records are append-oriented. A salary change creates a new record
rather than destroying the previous salary record.

### SalaryAudit

Stores a trace of salary changes.

Fields:

- Id
- SalaryId
- EmployeeId
- PreviousSalary
- NewSalary
- PreviousBonus
- NewBonus
- ChangeReason
- ChangedAt
- ChangedBy

## 5. Relationships

Country 1 ─── * Employee

Department 1 ─── * Employee

Employee 1 ─── * Salary

Employee 1 ─── * SalaryAudit

Salary 1 ─── * SalaryAudit

## 6. API Design

### Employees

GET /api/employees
GET /api/employees/{id}

Supports:

- pagination
- search
- country filtering
- department filtering
- employment-status filtering
- salary-range filtering
- sorting

### Salary

GET /api/employees/{id}/salary-history
POST /api/employees/{id}/salary

### Dashboard

GET /api/dashboard/summary
GET /api/dashboard/by-country
GET /api/dashboard/by-department

### Reference Data

GET /api/countries
GET /api/departments

### Export

GET /api/employees/export

Exports the currently filtered employee/salary dataset as CSV.

## 7. Pagination and Performance

Employee queries are paginated on the server.

The database query will apply filtering, sorting, and pagination before
materializing records.

Indexes will be added for frequently queried fields such as:

- EmployeeCode
- Email
- CountryId
- DepartmentId
- EmploymentStatus
- Salary.EmployeeId
- Salary.EffectiveFrom

The initial seed contains 10,000 employees, which is intentionally large
enough to validate server-side pagination and filtering without introducing
unnecessary infrastructure.

## 8. Currency Handling

Salary records retain their original currency.

The MVP will not depend on a live foreign-exchange API.

Where a consolidated payroll value is required, configured exchange rates
will be stored in the application/database and used deterministically.

Reports will also expose totals by original currency to avoid hiding currency
differences.

## 9. Error Handling

The API will return consistent HTTP status codes:

- 200 — successful read/update
- 201 — successful creation
- 400 — validation error
- 404 — resource not found
- 409 — business conflict
- 500 — unexpected server error

Validation errors will return structured responses suitable for displaying
messages in the Angular UI.

## 10. Testing Strategy

Core business logic will have unit tests.

Important scenarios include:

- employee search
- pagination
- employee filtering
- salary creation
- salary history preservation
- salary validation
- dashboard calculations
- salary audit creation

Integration tests will cover important API/database interactions where useful.

Tests should be deterministic and should not depend on external services.

## 11. Security Considerations

Salary information is sensitive.

The production design should include authentication and role-based
authorization for HR users.

Authentication/SSO is intentionally outside the MVP implementation so the
assessment can focus on salary management functionality.

The application will still avoid exposing internal database entities directly
through API responses.