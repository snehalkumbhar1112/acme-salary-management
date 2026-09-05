# Architecture and Engineering Trade-offs

## ReactJS vs AngularJS

ReactJS with TypeScript was selected for the UI because ReactJS is explicitly listed in the assessment requirements.

This keeps the frontend aligned with the stated technical constraint while providing a lightweight component-based architecture.

## ASP.NET Core .NET 8

ASP.NET Core .NET 8 was selected for the backend because it provides:
- Strong REST API support.
- Good performance.
- Built-in dependency injection.
- Strong Entity Framework Core integration.
- Long-term maintainability.

The assessment allows the backend framework to be selected according to the preferred JD or another framework.

## SQLite

SQLite was selected as the relational database because:
- It is simple to set up.
- It requires no separate database server.
- It is suitable for the assessment environment.
- It supports the required relational model and 10,000 employee dataset.

For a production deployment with significantly higher concurrency, a server-based relational database such as SQL Server or PostgreSQL would be a reasonable future choice.

## Bootstrap

Bootstrap was selected as the UI component and styling library because:
- It provides responsive components quickly.
- It reduces custom CSS requirements.
- It is easy to maintain.
- It allows the assessment UI to remain focused on functionality rather than visual complexity.

## Server-side Pagination

Employee data is paginated on the server instead of loading all 10,000 employees into the browser.

This reduces:
- Network payload size.
- Browser memory usage.
- Rendering work.
- Initial page load time.

## DTOs

DTOs are used instead of exposing database entities directly through API responses.

This provides:
- Clear API contracts.
- Separation between persistence models and API models.
- Reduced accidental exposure of database fields.
- Easier future API evolution.

## Salary History and Audit

Salary changes create a new salary record and an audit record instead of overwriting the previous salary.

This was chosen because HR salary data requires historical traceability.

The salary update and audit creation are performed within a database transaction so the operation can be rolled back if an error occurs.

## Scope Trade-off

The implementation intentionally focuses on the core HR salary-management workflow required by the assessment.

The following were deliberately left outside the initial scope:
- Authentication and authorization.
- Payroll processing.
- Tax calculation.
- Currency conversion.
- Employee self-service.
- External HR/payroll integrations.
- Advanced analytics.

These features would increase complexity without being necessary to demonstrate the core problem-solving and engineering requirements of the assessment.