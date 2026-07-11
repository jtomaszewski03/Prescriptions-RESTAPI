# Prescriptions API
PJATK APBD Exercise.
REST API for managing patients and medical prescriptions. The application was built with ASP.NET Core, Entity Framework Core and SQL Server.

## Features

- Create prescriptions with validation for due dates and medicament limits.
- Automatically create a patient record when a prescription is added for a new patient.
- Validate medicament identifiers before saving a prescription.
- Retrieve patient details with prescriptions, doctors and prescribed medicaments.
- Expose API documentation through Swagger UI in development mode.

## API Endpoints

| Method | Endpoint | Description |
| --- | --- | --- |
| `POST` | `/api/prescriptions` | Create a prescription with a new patient (if needed). |
| `GET` | `/api/patients/{id}` | Get patient details with prescriptions. |
| `DELETE` | `/api/patients/{id}` | Delete a patient. |

## Getting Started

1. Install .NET 10 SDK and SQL Server.
2. Update the `Default` connection string in `src/Prescriptions.Api/appsettings.json` if needed.
3. Apply database migrations:

```bash
dotnet ef database update --project src/Prescriptions.Api/Prescriptions.Api.csproj
```

4. Run the API:

```bash
dotnet run --project src/Prescriptions.Api/Prescriptions.Api.csproj
```

Swagger UI is available at `localhost:5000/swagger` in development mode.
