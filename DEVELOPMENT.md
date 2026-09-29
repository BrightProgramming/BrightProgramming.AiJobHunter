# Development

## Prerequisites

- .NET 10 SDK (the active LTS release)
- Docker Desktop with Docker Compose
- Git

## Start PostgreSQL

From the repository root:

```powershell
docker compose up -d postgres
docker compose ps
```

Wait until the `postgres` service reports `healthy`. If Docker cannot connect to its engine, start Docker Desktop and rerun the commands.

Local connection details:

- Host: `localhost`
- Port: `5432`
- Database: `aijobhunter`
- Username: `aijobhunter`
- Password: `local-development-only`

These credentials are only for local development. Do not reuse them outside a developer machine. The API reads its local connection string from `BrightProgramming.AiJobHunter.Api/appsettings.Development.json`.

## Run the API and create a job manually

Keep PostgreSQL running, then open a second terminal at the repository root and start the API:

```powershell
dotnet run --project BrightProgramming.AiJobHunter.Api --launch-profile http
```

On startup, DbUp applies any pending SQL scripts from `BrightProgramming.AiJobHunter.Api/Infrastructure/PostgreSQL/Migrations`. In Development, Swagger is available at <http://localhost:5263/swagger>. Use `POST /jobs` there to submit a job; a valid request returns `201 Created` and the new job ID. The request body has this shape:

```json
{
  "title": "Platform Engineer",
  "company": "Example Co",
  "url": "https://example.com/jobs/123"
}
```

You can also submit the same request from PowerShell:

```powershell
$job = @{
    title = "Platform Engineer"
    company = "Example Co"
    url = "https://example.com/jobs/123"
} | ConvertTo-Json

Invoke-RestMethod `
    -Uri "http://localhost:5263/jobs" `
    -Method Post `
    -ContentType "application/json" `
    -Body $job
```

Stop the API with **Ctrl+C** in its terminal.

## Stop PostgreSQL

Stop the container while retaining its data:

```powershell
docker compose down
```

Remove the container and its local database volume when you intentionally want a fresh database:

```powershell
docker compose down --volumes
```

## Build and test

```powershell
dotnet restore
dotnet build
dotnet test
```

Nullable reference analysis is enabled, and compiler/analyzer warnings fail the build. Fix warnings rather than suppressing them broadly.

## Working agreement

For each substantial feature, agree on the requirement and boundaries, implement one vertical slice, review the changes, and run the relevant tests. Keep changes focused and add external dependencies only when a feature needs them. Do not add microservices, an event bus, or a background scheduler without an agreed requirement.

## Next foundation steps

The API currently supports creating jobs. Add the next feature as an agreed, focused vertical slice, with tests.
