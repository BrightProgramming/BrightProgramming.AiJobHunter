# Architecture

## Initial shape

The system is a single deployable backend. The browser-facing Blazor Server UI calls the ASP.NET Core API over HTTP. The API owns HTTP handling, application logic, persistence, and any external integrations.

```text
Blazor Web UI  --HTTP-->  ASP.NET Core API
                              |       |
                              v       v
                           ASP.NET Core API
                                  |
                                  v
                              PostgreSQL
```

## Project responsibilities

- **Web**: presentation, navigation, and API client calls. It does not access the database directly.
- **Api**: HTTP endpoints, request/response handling, application logic, persistence, external integrations, and dependency composition.
- **Tests**: automated coverage for API behavior, including integration tests as those behaviors are implemented.

## Dependency rules

- Api is the backend project and has no project dependencies on other solution projects.
- Web has no backend project references; it communicates through the API.
- Tests may reference the layers they exercise.

## Deliberate constraints

Keep the backend in the API project. Do not introduce microservices, an event bus, or a background scheduler in the initial implementation. Add those only when concrete product needs justify the operational and design cost. Use PostgreSQL as the persistence platform; persistence code and schema migrations will be introduced in a later slice.

## Product boundary

The system is intended to help discover job opportunities, assess fit against a candidate profile, track applications, and identify applications that have stalled. The initial skeleton implements none of those features. Build them as small vertical slices, keeping deterministic business rules in C# and introducing AI only for tasks where it adds value.
