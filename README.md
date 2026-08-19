# Prescriptions API
PJATK APBD Exercise - later extended as a personal backend project.
REST API for managing patients and medical prescriptions. The application was built with ASP.NET Core, Entity Framework Core and SQL Server.

## Features

- Register users and authenticate them using JWT access tokens.
- Authorize access using the `User`, `Doctor`, and `Admin` roles.
- Create prescriptions with validation for due dates and medicament limits.
- Automatically create a patient when a prescription is issued for a new patient.
- Validate all medicaments before saving a prescription.
- Retrieve patient details with prescriptions, doctors, and prescribed medicaments.
- Delete patients using an administrator-only endpoint.
- Return consistent error responses using the `ProblemDetails` format.
- Run the API and SQL Server using Docker Compose.
- Apply Entity Framework Core migrations automatically when running in Docker.
- Verify the application with unit and integration tests.
- Build and test every change using GitHub Actions.

## Tech Stack

- .NET 10 and ASP.NET Core Web API
- Entity Framework Core 10
- SQL Server 2022
- JWT Bearer authentication
- xUnit, WebApplicationFactory, and SQLite
- Swagger / OpenAPI
- Docker and Docker Compose
- GitHub Actions

## API Endpoints

| Method | Endpoint | Access | Description |
| --- | --- | --- | --- |
| `POST` | `/api/auth/register` | Public | Register a new user account. |
| `POST` | `/api/auth/login` | Public | Authenticate and return a JWT token. |
| `GET` | `/api/auth/me` | Authenticated | Get information about the current user. |
| `POST` | `/api/prescriptions` | Doctor | Create a prescription for a new or existing patient. |
| `GET` | `/api/patients/{id}` | Doctor, Admin | Get patient details with prescriptions and medicaments. |
| `DELETE` | `/api/patients/{id}` | Admin | Delete a patient. |

## Getting Started

1. Install Docker Desktop.
2. Create a `.env` file in the repository root:

```env
MSSQL_SA_PASSWORD=YourStrongPassword123!
JWT_KEY=replace-this-with-a-random-key
```

3. Start the API and SQL Server:

```bash
docker compose up --build
```

Swagger UI will be available at [http://localhost:5000/swagger](http://localhost:5000/swagger).

## Tests

Run all unit and integration tests with:

```bash
dotnet test Prescriptions.Api.sln
```
