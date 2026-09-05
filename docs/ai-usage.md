# AI Usage and Development Approach

## Purpose

AI tools were used throughout the development process to accelerate implementation while keeping architectural decisions, validation, and correctness under human review.

## How AI Was Used

### 1. Requirements and Product Framing

AI was used to:
- Break down the problem statement into functional requirements.
- Identify the primary user persona as an HR Manager.
- Define the initial scope and deliberate exclusions.
- Identify the key workflows required for salary management.

### 2. Architecture and Design

AI was used to evaluate technology and architecture options and help structure the application.

The selected architecture consists of:
- ReactJS with TypeScript for the UI.
- ASP.NET Core .NET 8 Web API for the backend.
- Entity Framework Core for data access.
- SQLite as the relational database.
- Bootstrap for UI components and styling.

The final architecture was reviewed and adapted to the assessment requirements.

### 3. Implementation

AI assistance was used for:
- Generating initial project structure.
- Creating DTOs and entity-related code.
- Designing REST API endpoints.
- Implementing employee search, filtering, and pagination.
- Implementing salary history and salary updates.
- Implementing salary audit functionality.
- Implementing dashboard and salary reporting APIs.
- Building React pages and API service functions.

Generated code was reviewed, tested, and corrected when issues were identified.

### 4. Testing and Debugging

AI was used to:
- Identify implementation errors.
- Analyze API and database errors.
- Help diagnose SQLite limitations.
- Create unit test scenarios for core employee functionality.

Tests were executed locally using `dotnet test`, and failing behavior was corrected before proceeding.

### 5. Documentation

AI was used to help structure:
- Requirements documentation.
- Architecture documentation.
- Data model documentation.
- AI usage documentation.
- Trade-off documentation.
- Performance considerations.

The documentation reflects the actual implementation decisions made in the project.

## Example AI Instructions

Examples of instructions used during development included:

- "Design a salary management system for an organization with 10,000 employees."
- "Create a relational data model for employees, countries, departments, salaries, and salary audit history."
- "Implement server-side pagination and filtering for 10,000 employees."
- "Create REST APIs for employee details and salary management."
- "Review this implementation for correctness and maintainability."
- "Create meaningful unit tests for the core employee management functionality."
- "Analyze and fix the SQLite decimal aggregation issue."

## Human Review and Validation

AI-generated suggestions were not accepted blindly.

The implementation was validated by:
- Reviewing generated code.
- Running the application locally.
- Testing APIs through Swagger.
- Testing the React UI.
- Running unit tests.
- Checking database migrations and seed data.
- Reviewing errors and correcting implementation issues.

## Principle

AI was treated as a development accelerator and engineering assistant rather than as an autonomous decision-maker. Final technology choices, scope decisions, validation, and acceptance of generated code remained under human review.