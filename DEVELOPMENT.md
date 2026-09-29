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

Local connection details:

- Host: `localhost`
- Port: `5432`
- Database: `aijobhunter`
- Username: `aijobhunter`
- Password: `local-development-only`

These credentials are only for local development. Do not reuse them outside a developer machine. The app does not connect to the database in this skeleton.

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

The skeleton intentionally has no database provider, ORM, migrations, API health endpoint, or UI-to-API behavior. Add only the pieces needed for the first agreed vertical slice, with tests.
