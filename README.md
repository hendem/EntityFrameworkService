# EntityFrameworkService

## Project Description

This project is a comprehensive example of a modern .NET 8.0 Web API application using Entity Framework Core with ASP.NET Core. It demonstrates best practices for building a layered architecture with the classic Microsoft NorthWinds sample database.

## Architecture Overview

The solution follows a clean architecture pattern with clear separation of concerns:

### Project Structure

```
src/
├── EntityFrameworkService/           # Web API layer (Controllers, Startup, Program)
├── Application/
│   ├── Application.Models/          # DTOs and Models
│   └── Application.Services/        # Business logic services
├── Domain/                          # Domain entities and repository interfaces
└── Infrastructure/
    ├── NorthWinds.Persistence/      # Entity Framework implementation
    └── HealthChecks/                # Custom health check implementations

tests/
├── EntityFrameworkService_tests/    # Integration and API tests
├── Infrastructure/
│   ├── NorthWinds.Persistence.Tests/ # Persistence layer unit tests
│   └── HealthCheckTests/           # Health check tests
```

### Key Technologies

- **.NET 8.0** - Latest LTS version of .NET
- **ASP.NET Core 8.0** - Web framework
- **Entity Framework Core 8.0** - ORM with SQL Server provider
- **AutoMapper** - Object-to-object mapping
- **Swashbuckle/Swagger** - API documentation
- **Health Checks** - Application monitoring endpoints
- **Docker** - Containerization support
- **xUnit** - Testing framework

## Features

### API Endpoints

- **Customer Management** (`/api/Customer`)
  - Search customers by partial name or phone number
  - Paginated results with configurable page size
  
- **Orders Management** (`/api/Orders`)
  - Retrieve orders by customer ID
  - Retrieve orders by employee ID
  - Paginated results with configurable page size

- **Health Monitoring** (`/api/health`)
  - Database connectivity health checks
  - Application health status

- **Weather Forecast** (`/api/WeatherForecast`)
  - Sample endpoint for demonstration

### Database Features

- **NorthWinds Database** - Classic Microsoft sample database
- **Read/Write Context** - Full database operations
- **Read-Only Context** - Optimized for query operations
- **Entity Framework Migrations** - Database schema management
- **Connection String Management** - Separate connections for different operations

## Getting Started

### Prerequisites

- **.NET 8.0 SDK** or later
- **SQL Server** (LocalDB, Express, or full version)
- **Docker** (optional, for containerized deployment)

### Local Development Setup

1. **Clone the repository**
   ```bash
   git clone <repository-url>
   cd EntityFrameworkService
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore
   ```

3. **Configure the database**
   
   Update the connection strings in `src/EntityFrameworkService/appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "NorthWindsContext": "Server=localhost;Database=NorthWinds;User ID=aspnet;Password=devP@ssword!;TrustServerCertificate=True",
       "NorthWindsReadOnlyContext": "Server=localhost;Database=NorthWinds;User ID=aspnetReadOnly;Password=devP@ssword!;TrustServerCertificate=True"
     }
   }
   ```

4. **Set up the database**
   
   Run the provided database scripts in the `sql-test` folder:
   ```bash
   # Using PowerShell (Windows)
   cd sql-test
   .\init-database.ps1
   
   # Using Bash (Linux/macOS)
   cd sql-test
   ./Init-database.sh
   ```

5. **Apply Entity Framework migrations**
   ```bash
   cd src/Infrastructure/NorthWinds.Persistence
   dotnet ef database update
   ```

## Building the Application

### Command Line Build

```bash
# Build the entire solution
dotnet build

# Build in Release mode
dotnet build -c Release

# Publish the application
dotnet publish -c Release -o ./publish
```

### Docker Build

```bash
# Build Docker image
docker build -t entityframeworkservice:1.0.0 .

# Run the container
docker run -p 8080:8080 -p 8443:8443 entityframeworkservice:1.0.0
```

## Running the Application

### Local Development

```bash
# Run the application
cd src/EntityFrameworkService
dotnet run

# The API will be available at:
# HTTP: http://localhost:5000
# HTTPS: https://localhost:5001
# Swagger UI: http://localhost:5000/swagger
```

### Development with Hot Reload

```bash
cd src/EntityFrameworkService
dotnet watch run
```

### Docker

```bash
# Run with Docker Compose (if available)
docker-compose up

# Or run the built image directly
docker run -p 8080:8080 -p 8443:8443 entityframeworkservice:1.0.0
```

## Testing

### Running All Tests

```bash
# Run all tests
dotnet test

# Run tests with coverage
dotnet test --collect:"XPlat Code Coverage"

# Run tests with detailed output
dotnet test --verbosity normal
```

### Test Categories

1. **Unit Tests** - Testing individual components in isolation
2. **Integration Tests** - Testing API endpoints end-to-end
3. **Repository Tests** - Testing data access layer with in-memory database
4. **Health Check Tests** - Validating monitoring endpoints

## API Documentation

### Swagger/OpenAPI

When running locally, visit `http://localhost:5000/swagger` to access the interactive API documentation.

### Example API Calls

```bash
# Get customers with partial name search
curl "http://localhost:5000/api/Customer?partialName=al&pageNumber=1&resultsPerPage=10"

# Get orders by customer ID
curl "http://localhost:5000/api/Orders?customerId=ALFKI&pageNumber=1&resultsPerPage=5"

# Check application health
curl "http://localhost:5000/api/health"
```

## Configuration

### Environment Variables

- `ASPNETCORE_ENVIRONMENT` - Set to `Development`, `Staging`, or `Production`
- `ASPNETCORE_URLS` - Configure listening URLs
- `ConnectionStrings__NorthWindsContext` - Override database connection string
- `ConnectionStrings__NorthWindsReadOnlyContext` - Override read-only connection string

### Application Settings

Key configuration sections in `appsettings.json`:

- **ConnectionStrings** - Database connection strings
- **Logging** - Log levels and providers
- **JwtBearerOptions** - JWT authentication settings (commented out)
- **AuthorizationOptions** - Authorization policies (commented out)

## Database Management

### Entity Framework Migrations

```bash
# Create a new migration
cd src/Infrastructure/NorthWinds.Persistence
dotnet ef migrations add <MigrationName>

# Apply migrations to database
dotnet ef database update

# Generate SQL script for migrations
dotnet ef migrations script
```

### Database Scripts

The `sql-test` folder contains:
- `instnwnd.sql` - NorthWinds database schema
- `InsertNorthWindsDefaultData.sql` - Sample data
- `CreateUsers.sql` - Application database users
- `CreateReadOnlyUser.sql` - Read-only database user
- `CreateMigrationUsers.sql` - Migration database user

## Monitoring and Health Checks

The application includes comprehensive health checks:

- **Database Connectivity** - Validates both read/write and read-only connections
- **Custom Health Checks** - Template for additional checks
- **Health Check UI** - Available at `/health` endpoint

## Security Features

The application is prepared for:
- **JWT Bearer Authentication** (currently commented out)
- **Authorization Policies** (currently commented out)
- **HTTPS** - SSL certificate generation in Docker
- **Connection String Protection** - User secrets in development

## Performance Considerations

- **Separate Read/Write Contexts** - Optimized database access patterns
- **Pagination** - All list endpoints support paging
- **AutoMapper** - Efficient object mapping
- **Health Checks** - Monitor application performance

## Development Guidelines

### Code Organization

- **Controllers** - Thin controllers focused on HTTP concerns
- **Services** - Business logic implementation
- **Repositories** - Data access abstraction
- **DTOs** - Data transfer objects for API contracts
- **Entities** - Database entity models

### Best Practices

- Repository pattern for data access
- Service layer for business logic
- DTO pattern for API contracts
- Dependency injection throughout
- Comprehensive logging
- Unit and integration testing

## Troubleshooting

### Common Issues

1. **Database Connection Issues**
   - Verify SQL Server is running
   - Check connection strings
   - Ensure database users exist and have proper permissions

2. **Migration Issues**
   - Ensure connection string has appropriate permissions
   - Use the migration user for database updates

3. **Docker Issues**
   - Verify Docker is running
   - Check port availability
   - Ensure SSL certificate generation succeeds

### Logging

The application uses structured logging. Check logs for:
- Database connection issues
- API request/response information
- Health check failures
- General application errors

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Add or update tests
5. Ensure all tests pass
6. Submit a pull request

## License

This project is intended for educational and demonstration purposes, based on the classic Microsoft NorthWinds sample database.