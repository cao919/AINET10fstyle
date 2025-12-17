# AINET10fstyle

AINET10fstyle is an open-source project built on .NET, designed to provide a modular application architecture supporting features such as authentication, database migrations, and API services. This project is ideal for developers seeking to quickly establish a .NET-based microservices architecture.

## Project Structure

- **Zilor.AICopilot.AppHost** - The main host module for starting services.
- **Zilor.AICopilot.EntityFrameworkCore** - The data access layer, containing database contexts, migration scripts, and dependency injection configurations.
- **Zilor.AICopilot.HttpApi** - Provides HTTP API endpoints, including controllers, models, and foundational classes.
- **Zilor.AICopilot.IdentityService** - The authentication service module handling user creation and related operations.
- **Zilor.AICopilot.MigrationWorkApp** - The database migration and initialization module for executing migrations and seeding data.
- **Zilor.AICopilot.ServiceDefaults** - Provides common service defaults such as health checks and OpenTelemetry configurations.
- **Zilor.AICopilot.SharedKernel** - The shared core module containing common interfaces, result wrappers, and message handling utilities.

## Features

- Lightweight service architecture based on .NET Minimal API.
- Authentication system supporting user registration.
- Database migration and management using Entity Framework Core.
- Distributed tracing support via OpenTelemetry.
- Built-in health checks and default service configurations.
- Command and query handling using the MediatR pattern.

## Installation and Running

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- A database compatible with Entity Framework Core (e.g., SQLite, PostgreSQL, or SQL Server)

### Build the Project

```bash
dotnet restore
dotnet build
```

### Run Database Migrations

Navigate to the `src/Zilor.AICopilot.MigrationWorkApp` directory and run:

```bash
dotnet run
```

This will automatically execute database migrations and seed initial data.

### Start the Web API

Navigate to the `src/Zilor.AICopilot.HttpApi` directory and run:

```bash
dotnet run
```

The service will start on the default port. You can access endpoints such as `/api/identity/register`.

## Usage Example

### User Registration

Send a POST request to the `/api/identity/register` endpoint:

```json
{
  "username": "example",
  "password": "password"
}
```

## Contribution Guidelines

Contributions and documentation improvements are welcome. Please follow these steps:

1. Fork the project
2. Create a new branch (`git checkout -b feature/new-feature`)
3. Commit your changes (`git commit -am 'Add some feature'`)
4. Push to the branch (`git push origin feature/new-feature`)
5. Create a Pull Request

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.