# ACME Salary Management — Requirements

## 1. Goal

Build a web-based salary management system for ACME's HR Manager to replace spreadsheet-based salary management for approximately 10,000 employees across multiple countries.

The system should provide a centralized, searchable, auditable source of salary information and help HR understand how the organization compensates employees.

## 2. Primary User

**HR Manager**

The HR Manager should be able to manage employee salary information and answer questions such as:

- What is the average salary by country?
- What is the average salary by department?
- What is the total annual payroll?
- How are salaries distributed across salary ranges?
- Which employees recently received salary changes?

## 3. MVP Scope

### Employee Management

- View employees in a paginated table.
- Search by employee ID, name, email, department, and country.
- Filter by country, department, employment status, and salary range.
- View employee details.

### Salary Management

- View current salary information.
- Add salary records.
- Update salary information.
- Store base salary, bonus, currency, and effective date.
- Maintain salary history instead of overwriting previous compensation.

### Dashboard & Analytics

Provide:

- Total employee count.
- Total annual payroll.
- Average salary.
- Median salary.
- Salary distribution.
- Salary breakdown by country.
- Salary breakdown by department.
- Recent salary changes.

### Auditability

Salary changes should be traceable.

The system should record:

- Previous salary.
- New salary.
- Change date.
- Employee.
- Change reason.

### Data Operations

- Seed the application with 10,000 employees.
- Support CSV export of filtered employee/salary data.

## 4. Non-Goals

The following are intentionally excluded from the MVP:

- Payroll processing.
- Payslip generation.
- Tax calculation.
- Benefits management.
- Attendance and leave management.
- Employee self-service.
- Real identity-provider/SSO integration.
- Complex approval workflows.
- Real-time foreign exchange conversion.

These features introduce additional domain and infrastructure complexity without being necessary to demonstrate the core salary-management workflow. They can be considered in future versions.

## 5. Product & Engineering Decisions

1. Salary records are separate from employees so salary history is preserved.
2. Currency is stored with each salary record to support multiple countries.
3. Employee search, filtering, sorting, and pagination are performed server-side so the UI does not load all 10,000 employees at once.
4. Dashboard metrics use the same salary data source as operational screens.
5. Salary changes are auditable because compensation data is sensitive and historical changes should be traceable.
6. The application will initially use a modular monolith because the domain and expected scale do not justify microservices.

## 6. Success Criteria

The MVP is successful when an HR Manager can:

- Find an employee quickly.
- View current and historical compensation.
- Add or update salary without losing historical information.
- Filter employees across countries, departments, and salary ranges.
- Understand organizational salary patterns through the dashboard.
- Export filtered salary information.
- Perform these operations reliably with 10,000 seeded employees.