# ACME Salary Management — Requirements

## 1. Goal

Build a web-based salary management system for ACME's HR Manager to manage and analyze salary information for an organization of approximately 10,000 employees across multiple countries.

The system replaces spreadsheet-based salary management with a centralized application that provides employee salary information, salary updates, salary history, auditability, and organization-level salary insights.

## 2. User Persona

**Primary user:** HR Manager

The HR Manager needs to:
- Find employees quickly.
- View employee and salary information.
- Update employee salaries while preserving salary history.
- Understand how compensation is distributed across countries and departments.
- Review salary changes for audit purposes.

## 3. MVP Scope

### Employee Management
- View employees in a paginated list.
- Search employees by relevant employee information.
- Filter employees by country, department, and employment status.
- View employee details and current salary information.

### Salary Management
- Update an employee's base salary and bonus.
- Record the reason for a salary change.
- Preserve previous salary records as salary history.
- Record salary changes for audit purposes.

### Salary Analytics
Provide HR with organization-level salary information including:
- Total employee count.
- Total base salary.
- Total bonus.
- Total compensation.
- Salary breakdown by country.
- Salary breakdown by department.

### Data and Deployment
- Relational database for employee, organizational, salary, and audit data.
- Deterministic seed data for approximately 10,000 employees.
- Fully functional deployed frontend and backend.

## 4. Out of Scope

The following are deliberately excluded from the MVP:

- Employee self-service.
- Payroll processing or salary payment integration.
- Authentication and role-based access control.
- Benefits and deductions management.
- Tax calculation.
- Performance management.
- Recruitment functionality.
- Email/notification workflows.
- CSV/export functionality.
- Advanced statistical salary analysis such as predictive analytics.

### Reasoning

The assessment focuses on demonstrating strong product thinking and engineering fundamentals rather than building a complete HR/payroll platform. These features increase scope and complexity without being necessary to solve the core problem of replacing spreadsheet-based salary management and helping HR understand organizational compensation.

## 5. Non-Functional Expectations

- Support approximately 10,000 employees.
- Use server-side pagination for employee data.
- Keep API responses focused on the data required by the UI.
- Preserve salary history rather than overwriting historical records.
- Maintain an audit trail for salary changes.
- Keep the codebase structured, readable, and maintainable.
- Provide meaningful automated tests for core functionality.
- Ensure the deployed application is usable end-to-end.

## 6. Success Criteria

The MVP is successful when an HR Manager can:

1. Find and view employee salary information.
2. Filter and navigate a large employee dataset efficiently.
3. Update salary information with a recorded reason.
4. Review previous salary information and salary-change audit records.
5. Understand total compensation and salary distribution by country and department.
6. Use the application successfully in the deployed environment.