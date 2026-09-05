# Data Model

## 1. Overview

The Salary Management System uses a relational data model designed to support:

- 10,000+ employees
- Employees across multiple countries
- Department-based organization
- Salary history and effective dates
- Salary change auditing
- Efficient employee filtering and pagination

SQLite is used as the relational database for the assessment because it is lightweight, easy to deploy, and sufficient for the current scale.

---

## 2. Entities

### Country

Stores countries in which employees are located.

| Field | Type | Description |
|---|---|---|
| Id | int | Primary key |
| Name | string | Country name |
| Code | string | Unique country code |
| CurrencyCode | string | Salary currency |

Relationship:

- One Country can have many Employees.

---

### Department

Stores organizational departments.

| Field | Type | Description |
|---|---|---|
| Id | int | Primary key |
| Name | string | Unique department name |

Relationship:

- One Department can have many Employees.

---

### Employee

Stores employee master information.

| Field | Type | Description |
|---|---|---|
| Id | int | Primary key |
| EmployeeCode | string | Unique employee identifier |
| FirstName | string | Employee first name |
| LastName | string | Employee last name |
| Email | string | Unique employee email |
| CountryId | int | Foreign key to Country |
| DepartmentId | int | Foreign key to Department |
| JobTitle | string | Employee job title |
| EmploymentStatus | string | Current employment status |
| HireDate | DateTime | Employee joining date |
| CreatedAt | DateTime | Record creation timestamp |
| UpdatedAt | DateTime | Last update timestamp |

Relationships:

- Employee belongs to one Country.
- Employee belongs to one Department.
- Employee can have many Salary records.
- Employee can have many SalaryAudit records.

---

### Salary

Stores salary records and salary history.

| Field | Type | Description |
|---|---|---|
| Id | int | Primary key |
| EmployeeId | int | Foreign key to Employee |
| BaseSalary | decimal | Base salary amount |
| Bonus | decimal | Bonus amount |
| CurrencyCode | string | Salary currency |
| EffectiveFrom | DateTime | Date from which salary is effective |
| EffectiveTo | DateTime? | Date until which salary is effective |
| ChangeReason | string | Reason for salary change |
| CreatedAt | DateTime | Record creation timestamp |

Relationships:

- Salary belongs to one Employee.
- Salary can have salary audit records.

### Salary History Design

Salary changes are stored as new records instead of overwriting the previous salary.

Example:

```text
Salary 1
BaseSalary: 709,518
EffectiveFrom: 2026-01-01
EffectiveTo: 2026-09-05

Salary 2
BaseSalary: 800,000
EffectiveFrom: 2026-09-05
EffectiveTo: NULL