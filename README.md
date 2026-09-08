# acme-salary-management

Employee salary management platform built as a product engineering assessment.

## Overview

This application provides a simple employee salary management system with:

* Employee listing and employee details
* Employee salary history
* Salary updates with change reasons
* Salary audit history
* Dashboard summary
* Salary analysis by country
* Salary analysis by department
* Country and department reference data
* RESTful API endpoints
* Swagger/OpenAPI API documentation

## Technology Stack

### Backend

* ASP.NET Core / .NET 8
* Entity Framework Core
* SQL Server
* RESTful Web API
* Swagger / OpenAPI

### Frontend

* Angular
* TypeScript
* Bootstrap

### Testing

* xUnit
* Entity Framework Core InMemory
* ASP.NET Core controller tests

## Project Structure

```text
acme-salary-management/
├── backend/
│   └── AcmeSalary.Api/
├── frontend/
├── tests/
│   └── AcmeSalary.Api.Tests/
└── docs/
```

## Main API Endpoints

| Method | Endpoint                              | Description                     |
| ------ | ------------------------------------- | ------------------------------- |
| GET    | `/api/Employees`                      | Get employees                   |
| GET    | `/api/Employees/{id}`                 | Get employee details            |
| GET    | `/api/Employees/{id}/salary-history`  | Get salary history              |
| POST   | `/api/Employees/{id}/salary`          | Update employee salary          |
| GET    | `/api/Employees/{id}/salary-audit`    | Get salary audit history        |
| GET    | `/api/Dashboard/summary`              | Get dashboard summary           |
| GET    | `/api/Dashboard/salary-by-country`    | Get salary totals by country    |
| GET    | `/api/Dashboard/salary-by-department` | Get salary totals by department |
| GET    | `/api/Countries`                      | Get countries                   |
| GET    | `/api/Departments`                    | Get departments                 |

## Running the Backend

From the repository root:

```powershell
cd backend\AcmeSalary.Api
dotnet restore
dotnet build
dotnet run
```

The API can then be accessed using the configured local application URL.

Swagger UI is available through:

```text
/swagger
```

## Running the Frontend

From the frontend directory:

```powershell
cd frontend
npm install
ng serve
```

The Angular application will be available at the local development URL shown by Angular CLI.

## Running Tests

From the repository root:

```powershell
dotnet test .\tests\AcmeSalary.Api.Tests\AcmeSalary.Api.Tests.csproj
```

The current automated test suite covers:

* Employee details for an existing employee
* Employee not-found handling
* Dashboard summary calculations
* Salary totals by country
* Salary totals by department

## Assessment Documentation

Additional assessment documentation is available in the `docs` directory.

## Deployment

The application is deployed with separate frontend and backend services.

Backend:

```text
https://acme-salary-management-hfgm.onrender.com
```

Frontend:

```text
https://acme-salary-management-1.onrender.com
```

## Notes

This project focuses on the functionality required by the assessment and intentionally avoids unrelated additional features.
