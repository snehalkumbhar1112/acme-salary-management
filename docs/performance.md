# Performance Considerations

## Dataset Size

The system is designed for the assessment requirement of 10,000 employees.

The seed process creates 10,000 employees with an initial salary record for each employee.

## Employee Listing

Employee listing uses server-side pagination.

The API:
- Applies filters before pagination.
- Uses `CountAsync()` for the total record count.
- Uses `Skip()` and `Take()` for pagination.
- Returns only the requested page of employees.

The API also limits the maximum page size to 100 records.

This prevents the application from loading all 10,000 employees into the browser.

## Filtering

Employee search and filters are applied at the database query level.

Supported filters include:
- Employee code.
- First name.
- Last name.
- Email.
- Country.
- Department.
- Employment status.

Database indexes were added to commonly filtered fields such as employee code, email, country, department, and employment status.

## Salary Queries

Salary history queries are filtered by employee and ordered by effective date.

An index on `EmployeeId` and `EffectiveFrom` supports these access patterns.

## Dashboard and Reports

Dashboard summary queries use database count operations rather than loading all employee records into application memory.

Salary report queries retrieve the required salary data and then perform the final grouping and aggregation in application memory where required by SQLite's decimal aggregation limitations.

This approach is acceptable for the assessment dataset of 10,000 employees.

## Frontend Performance

The React application requests only the data required by the current page.

Employee pagination prevents large datasets from being rendered simultaneously.

Independent dashboard/report requests can be executed in parallel using `Promise.all()` where appropriate.

## Deterministic Seed Data

The seed process uses a deterministic random seed.

This makes the generated 10,000-employee dataset reproducible, which helps development and testing.

## Future Production Improvements

For a significantly larger production workload, the following could be considered:
- SQL Server or PostgreSQL instead of SQLite.
- Additional composite indexes based on production query patterns.
- Database-level aggregation for reporting.
- Caching for frequently requested dashboard summaries.
- Asynchronous background processing for expensive reports.
- Monitoring and query performance profiling.

These improvements were intentionally not added because the assessment focuses on a 10,000-employee working solution rather than production infrastructure at enterprise scale.