# BrightProgramming.AiJobHunter

A personal job discovery and application management project, built in small, reviewable vertical slices.

## Current milestone

This repository contains the initial .NET solution structure, a Blazor Server UI shell, an ASP.NET Core API shell, a test project, and PostgreSQL for local development. It deliberately contains no job-search, AI, matching, candidate-profile, or application-tracking behavior yet.

## Projects

- `BrightProgramming.AiJobHunter.Web` — Blazor Server user interface.
- `BrightProgramming.AiJobHunter.Api` — ASP.NET Core HTTP API.
- `BrightProgramming.AiJobHunter.UnitTests` — automated unit tests.
- `BrightProgramming.AiJobHunter.IntegrationTests` — automated integration tests.

See [ARCHITECTURE.md](ARCHITECTURE.md) for boundaries and [DEVELOPMENT.md](DEVELOPMENT.md) for local setup.

## Quick start

Requirements: .NET 10 SDK and Docker Compose.

```powershell
docker compose up -d postgres
dotnet restore
dotnet build
dotnet test
```

The database is available at `localhost:5432` with database `aijobhunter` and the local-only credentials shown in `docker-compose.yml`. No application project connects to PostgreSQL yet.
