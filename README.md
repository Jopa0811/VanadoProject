# Machine Fault Tracker

A small full-stack .NET app for tracking machines and their faults ("Stroj" = machine, "Kvar" = fault). Originally built as a coding assignment; it is meant to showcase backend design with a REST API, a relational database, real-time updates and tests.

## What it shows

- **REST API** (ASP.NET Core, .NET 8) with layered architecture: Controller → Service → Repository → data access
- **PostgreSQL** with versioned **DbUp** migrations, custom enum types and **stored procedures** (PL/pgSQL), accessed via **Dapper/Npgsql**
- **Concurrency handling**: pessimistic locking (`SELECT ... FOR UPDATE`) in update/delete procedures and a transactional delete
- **Business rules in the database**: unique machine names, only one active fault per machine
- **SignalR**: the API broadcasts `KvarDodan` / `KvarStatusPromijenjen` events, and the Blazor client updates live without polling
- **Security**: Basic-auth login issues a **JWT** (1 h); all resource endpoints require a bearer token; global **rate limiting**
- **Clean API plumbing**: a shared `ApiBase` maps exceptions to HTTP status codes and triggers hub notifications
- **Query features**: paged list sorted by priority, per-machine details with average fault duration
- **Tests**: xUnit + Moq for services and repositories
- **Swagger/OpenAPI** documentation

## Structure

| Project | Purpose |
|---|---|
| `VanadoProject` | ASP.NET Core Web API, controllers, SignalR hub, auth |
| `VanadoShared` | Models, services, repositories, Postgres data access |
| `VanadoMigration` | DbUp console app with SQL migrations |
| `VanadoBlazorApp` | Blazor client with live fault list |
| `TestVanado` | Unit tests |

## Running locally

1. Start PostgreSQL and create a database. Set the same connection string in `VanadoProject/appsettings.json` and `VanadoMigration/Program.cs`.
2. Run the migrations: `dotnet run --project VanadoMigration`
3. Start the API: `dotnet run --project VanadoProject` (Swagger at `https://localhost:7076/swagger`)
4. Get a token: `POST /api/login` with Basic auth `admin:admin` (hard-coded demo user), then send it as `Authorization: Bearer <token>`.
5. Start the client: `dotnet run --project VanadoBlazorApp/VanadoBlazorApp` (the API allows CORS from `https://localhost:7256`).

> Credentials and the JWT key in this repo are demo values for local use only.
