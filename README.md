# GDC.EventHost.API

## Overview

GDC.EventHost.API is a RESTful API built with ASP.NET Core 8.0 for managing events, performances, venues, ticketing, and orders. It is designed to serve as the backend for an event hosting platform.

## Technology Stack

- **Framework:** ASP.NET Core 8.0
- **Database:** SQL Server (via Docker on Mac, LocalDB on Windows)
- **ORM:** Entity Framework Core 9.0
- **Authentication:** ASP.NET Core Identity with self-signed JWT bearer tokens
- **Logging:** Serilog
- **Mapping:** AutoMapper
- **API Documentation:** Swagger / OpenAPI
- **API Versioning:** Asp.Versioning

## Projects

| Project | Description |
|---|---|
| `GDC.EventHost.API` | ASP.NET Core Web API — controllers, services, profiles |
| `GDC.EventHost.DAL` | Data access layer — DbContext, entities, migrations, seed data |
| `GDC.EventHost.Shared` | Shared DTOs and validation attributes |

## Authentication

The API uses JWT bearer token authentication. All versioned endpoints require a valid token.

**Register a user:**
```http
POST /api/authentication/register
```
```json
{
  "username": "yourname",
  "email": "you@example.com",
  "password": "Password1",
  "firstName": "First",
  "lastName": "Last",
  "city": "Seattle",
  "isAdmin": false
}
```

**Authenticate and get a token:**
```http
POST /api/authentication/authenticate
```
```json
{
  "username": "yourname",
  "password": "Password1"
}
```

Include the returned token in subsequent requests:
```
Authorization: Bearer <token>
```

## API Endpoints

All versioned endpoints are prefixed with `/api/v1/`.

| Resource | Base Route | Operations |
|---|---|---|
| Series | `/series` | GET, POST, PUT, PATCH, DELETE |
| Series Assets | `/series/{id}/assets` | GET, POST |
| Events | `/events` | GET, POST, PUT, PATCH, DELETE |
| Event Assets | `/events/{id}/assets` | GET, POST |
| Performances | `/performances` | GET, POST, PUT, PATCH, DELETE |
| Performance Types | `/performancetypes` | GET |
| Venues | `/venues` | GET, POST, PUT, PATCH, DELETE |
| Venue Assets | `/venues/{id}/assets` | GET, POST |
| Seating Plans | `/seatingplans` | GET, POST, PUT, PATCH, DELETE |
| Seat Positions | `/seatpositions` | GET |
| Seats | `/seats` | GET, POST, PUT, PATCH, DELETE |
| Tickets | `/tickets` | GET, POST, PUT, PATCH, DELETE |
| Members | `/members` | GET, POST, PUT, PATCH, DELETE |
| Orders | `/orders` | GET, POST, PUT, PATCH, DELETE |
| Order Items | `/orderitems` | GET, POST, DELETE |
| Shopping Carts | `/shoppingcarts` | GET, POST, PUT, DELETE |
| Shopping Cart Items | `/shoppingcartitems` | GET, POST, DELETE |
| Assets | `/assets` | GET, POST, PUT, DELETE |

## Query Parameters

Several endpoints support filtering and pagination via query parameters:

| Parameter | Description |
|---|---|
| `title` | Filter by title |
| `searchQuery` | Search across text fields |
| `pageNumber` | Page number (default: 1) |
| `pageSize` | Results per page (default: 10) |
| `includePast` | Include past events/performances (default: false) |

## Local Development Setup

### Prerequisites
- .NET 8.0 SDK
- Docker Desktop (for SQL Server on Mac)
- `dotnet-ef` tools: `dotnet tool install --global dotnet-ef`

### Database
Start SQL Server in Docker:
```bash
docker run -e "ACCEPT_EULA=Y" -e "SA_PASSWORD=YourPassword!" \
  -p 1433:1433 --name eventhost-sql -d mcr.microsoft.com/azure-sql-edge
```

### User Secrets
Configure the connection string and JWT settings via user secrets:
```bash
dotnet user-secrets set "ConnectionStrings:EventHostDBConnectionString" \
  "Server=localhost,1433;Database=EventHostDb-Local;User Id=sa;Password=YourPassword!;TrustServerCertificate=True;" \
  --project GDC.EventHost.API

dotnet user-secrets set "Authentication:Issuer" "https://localhost:7001" --project GDC.EventHost.API
dotnet user-secrets set "Authentication:Audience" "eventhostapi" --project GDC.EventHost.API
dotnet user-secrets set "Authentication:SecretForKey" "<base64-encoded-key>" --project GDC.EventHost.API
```

### Run Migrations
```bash
dotnet ef database update --project GDC.EventHost.DAL --startup-project GDC.EventHost.API
```

### Run the API
```bash
dotnet run --project GDC.EventHost.API --launch-profile http
```

Swagger UI is available at `http://localhost:5090/swagger`.
