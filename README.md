# RuyaOptik API (ASP.NET Core 8) — Backend Development Project

> **Course project** (Backend Development) developed by **Hakan Onay** and **Mehmet Çakmak**.  
> This repository contains the backend API of **RuyaOptik**, an optical e-commerce style system including authentication/authorization, product & category management, cart and order flows, inventory tracking, caching, logging, and real-time order notifications with SignalR.

---

## Table of Contents

- [Project Summary](#project-summary)
- [Tech Stack](#tech-stack)
- [Key Features](#key-features)
- [Architecture Overview](#architecture-overview)
- [Solution / Folder Structure](#solution--folder-structure)
- [Getting Started (Local)](#getting-started-local)
- [Running with Docker](#running-with-docker)
- [Configuration](#configuration)
  - [Connection String Resolution](#connection-string-resolution)
  - [JWT Settings](#jwt-settings)
  - [CORS](#cors)
  - [Mail Settings](#mail-settings)
- [Database & Migrations](#database--migrations)
- [Swagger / Authorization](#swagger--authorization)
- [Endpoints Overview](#endpoints-overview)
- [Authorization Definitions & Role Permissions](#authorization-definitions--role-permissions)
- [Caching Strategy](#caching-strategy)
- [Logging & Observability](#logging--observability)
- [Real-time Notifications (SignalR)](#real-time-notifications-signalr)
- [Error Handling](#error-handling)
- [Notes / Known Considerations](#notes--known-considerations)
- [Contributors](#contributors)

---

## Project Summary

**RuyaOptik API** is a RESTful backend built with **ASP.NET Core 8**.  
It provides endpoints for:

- User registration and login (JWT-based)
- Role management and dynamic endpoint permission checks
- Product browsing with filtering/sorting/pagination
- Category management
- Shopping cart operations (add/remove/clear)
- Order creation using **database transactions**
- Inventory reservations and stock update on order status changes
- Cache & response caching to improve performance
- Logging (Serilog) + HTTP logging for diagnostics
- Real-time order notifications for admins via **SignalR**

---

## Tech Stack

- **.NET / ASP.NET Core 8**
- **Entity Framework Core** (SQL Server provider)
- **ASP.NET Core Identity** (users/roles, token providers)
- **JWT Bearer Authentication**
- **AutoMapper**
- **IMemoryCache** + custom cache versioning
- **Response Caching Middleware**
- **Serilog** (console + rolling file logs)
- **ASP.NET Core HTTP Logging**
- **Swagger / OpenAPI** with Bearer auth support
- **SignalR** for real-time admin notifications
- **Docker** (multi-stage build)

---

## Key Features

### Authentication & Authorization
- **JWT-based login** (`/api/auth/login`) returning `accessToken` + `refreshToken`.
- **Refresh token flow** (`/api/auth/refresh-token`).
- Password reset flow:
  - `/api/auth/password-reset`
  - `/api/auth/verify-reset-token`
  - `/api/users/update-password`
- Role-based protection using `[Authorize(Roles="...")]`.

### Role & Permission System (Endpoint-based)
- Custom `[AuthorizeDefinition]` attribute (Menu/Definition/ActionType) on endpoints.
- `ApplicationService` scans controllers at runtime and collects permission definitions.
- `RolePermissionFilter` checks if a user has permission for a given endpoint using a computed **endpoint code**.

### E-commerce Modules
- **Category** CRUD (admin-only for write/update/delete).
- **Products** with filter/sort/pagination:
  - filter by category/brand/min-max price/active/search
  - sort: newest, oldest, price asc/desc
- **Cart** for authenticated users (add/remove/clear).
- **Inventory** management (admin-only).
- **Orders**
  - order creation uses **transaction**
  - reserves inventory items
  - clears cart on successful order
  - updates inventory on status changes (Delivered/Cancelled)

### Performance Improvements
- **In-memory caching** for:
  - product list pages (versioned)
  - product by id
  - categories list (versioned)
  - inventory by product
- **Response caching** for some GET endpoints (e.g., category & product listing).

### Logging & Diagnostics
- Serilog console + rolling file logs (`logs/log.txt`).
- HTTP logging with sensitive headers removed.

### Real-time Updates
- SignalR Hub (`/hubs/orders`) for **Admin** real-time order notifications.

---

## Architecture Overview

The solution follows a layered approach:

- **API Layer** (`RuyaOptik.API`)
  - Controllers, middleware, filters, hubs
  - Startup configuration (`Program.cs` + `ServiceExtensions`)
- **Business Layer** (`RuyaOptik.Business`)
  - Service interfaces & implementations
  - Token service, auth logic, caching version service
  - Custom exceptions
- **DataAccess Layer** (`RuyaOptik.DataAccess`)
  - EF Core DbContext, generic repository pattern
  - Concrete repositories for domain entities
  - Configuration for connection string + Identity role seeding
- **Entity Layer** (`RuyaOptik.Entity`)
  - Domain entities (Product, Category, Cart, Order, Inventory, Endpoint)
  - Identity entities (AspUser, AspRole)
  - Enums and configuration classes (Menu, Action)
- **DTO Layer** (`RuyaOptik.DTO`)
  - Request/response models for API

---

## Solution / Folder Structure

High-level structure:

```
RuyaOptik.API/
  Controllers/
  Extensions/
  Filters/
  Hubs/
  Middlewares/
  Program.cs

RuyaOptik.Business/
  Interfaces/
  Services/
  Attributes/
  Consts/
  Exceptions/
  Mapping/

RuyaOptik.DataAccess/
  Context/
  Repositories/
    Concrete/
    Interfaces/
  Repositories/Configuration/

RuyaOptik.Entity/
  Common/
  Concrete/
  Configurations/
  Enums/
  Identity/

RuyaOptik.DTO/
  Auth/
  Cart/
  Category/
  Common/
  Inventory/
  Order/
  Product/
  Role/
  SignalR/
  System/
  User/
```

---

## Getting Started (Local)

### Prerequisites
- .NET SDK **8.0**
- SQL Server (LocalDB / Express / Docker SQL Server)
- Optional: Docker Desktop

### Steps
1. Restore & build:
   ```bash
   dotnet restore
   dotnet build
   ```

2. Update `appsettings.json` **connection string** and **JWT** settings (see [Configuration](#configuration)).

3. Apply EF Core migrations:
   ```bash
   dotnet ef database update --project RuyaOptik.DataAccess --startup-project RuyaOptik.API
   ```

4. Run:
   ```bash
   dotnet run --project RuyaOptik.API
   ```

5. Open Swagger:
   - `http://localhost:<port>/swagger`

---

## Running with Docker

This project includes a `Dockerfile` and a minimal `docker-compose.yml`.

### docker-compose.yml (API container)
The compose file maps:
- Host `8080` → Container `8080`

Run:
```bash
docker compose up --build
```

Then:
- API: `http://localhost:8080`
- Swagger: `http://localhost:8080/swagger`

> The container is configured with:
> `ASPNETCORE_URLS=http://+:8080`

If you want to connect to SQL Server from Docker, provide a connection string via environment variable:
- `SQL_CONNECTION_STRING`

(See [Connection String Resolution](#connection-string-resolution))

---

## Configuration

### Connection String Resolution

The connection string is resolved in this order:

1. **Environment variable** (recommended for Docker/Prod):
   - `SQL_CONNECTION_STRING`

2. Otherwise, from `appsettings.json`:
   - `ConnectionStrings:SqlConnection`

Implemented in:
- `RuyaOptik.DataAccess.Repositories.Configuration.AppConfiguration`

### JWT Settings

JWT settings are read from configuration:
- `Jwt:Issuer`
- `Jwt:Audience`
- `Jwt:Key`

Used in:
- `ServiceExtensions.ConfigureAuthentication()`

⚠️ `Jwt:Key` must be long/strong enough for HMAC SHA-256.

### CORS

CORS policy allows:
- `http://localhost:5188` (local dotnet run)
- `http://localhost:8080` (docker)

And enables:
- Any method, any header, credentials

Configured in:
- `ServiceExtensions.ConfigureCors()`

### Mail Settings

Mail is sent via SMTP and reads:
- `Mail:Username`
- `Mail:Password`
- `Mail:Host`
- `ClientUrl` (used for password reset link)

Implemented in:
- `RuyaOptik.Business.Services.MailService`

---

## Database & Migrations

### DbContext
- `RuyaOptikDbContext : IdentityDbContext<AspUser, AspRole, string>`

### Seeded Roles
Identity roles are seeded:
- `User`
- `Admin`

Via:
- `RoleConfiguration`

### Migrations
Create a migration:
```bash
dotnet ef migrations add Init --project RuyaOptik.DataAccess --startup-project RuyaOptik.API
```

Apply:
```bash
dotnet ef database update --project RuyaOptik.DataAccess --startup-project RuyaOptik.API
```

---

## Swagger / Authorization

Swagger is enabled in **Development** environment:
- `app.UseSwagger()`
- `app.UseSwaggerUI()`

Swagger includes a **Bearer** security definition, so you can:

1. Login using:
   - `POST /api/auth/login`
2. Copy the returned `accessToken`
3. Click **Authorize** in Swagger
4. Paste:
   - `Bearer <token>`

---

## Endpoints Overview

High-level endpoint list (based on current controllers). DTOs are in `RuyaOptik.DTO`.

### AuthController (`/api/auth`)
- `POST /api/auth/login`
- `POST /api/auth/refresh-token`
- `POST /api/auth/password-reset`
- `POST /api/auth/verify-reset-token`

### UsersController (`/api/users`)
- `POST /api/users/register`
- `GET /api/users/get-users` (Admin)
- `POST /api/users/{userId}/roles` (Admin)
- `GET /api/users/get-roles-from-user/{userIdOrName}` (Admin)
- `POST /api/users/update-password`

### RolesController (`/api/roles`) (Admin)
- `GET /api/roles`
- `GET /api/roles/{id}`
- `POST /api/roles`
- `PUT /api/roles/{id}`
- `DELETE /api/roles/{id}`

### CategoryController (`/api/category`)
- `GET /api/category` (ResponseCache 60s)
- `GET /api/category/{id}`
- `POST /api/category` (Admin)
- `PUT /api/category/{id}` (Admin)
- `DELETE /api/category/{id}` (Admin)

### ProductController (`/api/product`)
- `GET /api/product?page=1&pageSize=10&search=ray&sort=PriceAsc`
- `GET /api/product/{id}` (ResponseCache 60s)
- `POST /api/product` (Admin)
- `PUT /api/product/{id}` (Admin)
- `DELETE /api/product/{id}` (Admin)

### InventoryController (`/api/inventory`) (Admin)
- `GET /api/inventory/product/{productId}`
- `POST /api/inventory`
- `PUT /api/inventory/{id}`

### CartController (`/api/cart`) (Authorized)
- `GET /api/cart/user/{userId}`
- `POST /api/cart/add`
- `DELETE /api/cart/item/{cartItemId}`
- `DELETE /api/cart/clear/{userId}`

### OrderController (`/api/order`)
- `POST /api/order` (Admin, User) + SignalR notification
- `GET /api/order/user/{userId}` (Admin, User)
- `PUT /api/order/{id}/status` (Admin)

### ApplicationServiceController (`/api/applicationservice`) (Admin)
- `GET /api/applicationservice/authorize-definitions`

### InfoController (`/api/info`) (Admin)
- `GET /api/info`

---

## Authorization Definitions & Role Permissions

### 1) `[AuthorizeDefinition]` attribute
Endpoints can be decorated with:
- `ActionType` (`Reading/Writing/Updating/Deleting`)
- `Menu` name (e.g., `Product`, `Category`, `Order`)
- Human-readable `Definition`

Example:
```csharp
[AuthorizeDefinition(
    Action = ActionType.Writing,
    Definition = "Ürün Oluştur",
    Menu = AuthorizeDefinitionConstants.Product)]
```

### 2) Endpoint Code
Both `ApplicationService` and `RolePermissionFilter` compute:

```
{HttpMethod}.{ActionType}.{DefinitionWithoutSpaces}
```

Example:
```
POST.Writing.ÜrünOluştur
```

### 3) RolePermissionFilter
At runtime:
- reads current username (`ClaimTypes.Name`)
- builds endpoint code
- checks `IUserService.HasRolePermissionToEndpointAsync(username, code)`
- returns `401 Unauthorized` if permission is missing

---

## Caching Strategy

### In-memory cache keys
`CacheKeys` contains patterns like:
- `products:version`, `categories:version` (global invalidation)
- `products:id:{id}`
- `products:list:v{version}:{key}`
- `categories:all:v{version}`
- `inventory:product:{productId}`

### Cache versioning
`CacheVersionService` keeps an integer version.
On create/update/delete of products/categories the version increments, invalidating list caches.

### Response caching
Some endpoints use `[ResponseCache]` (e.g., categories/products GET).

---

## Logging & Observability

### Serilog
Configured in `ServiceExtensions.ConfigureSerilog()`:
- Console sink
- Rolling file sink: `logs/log.txt` (daily, keep last 7)

(SQL sink exists but is commented.)

### HTTP Logging
Configured in `ServiceExtensions.ConfigureHttpLogging()`:
- Logs method/path/headers and status/headers
- Removes sensitive headers: `Authorization`, cookies, etc.

---

## Real-time Notifications (SignalR)

### Hub
- `/hubs/orders`
- Protected: **Admin** role required.

Admins are added to:
- `OrdersHub.AdminGroup = "admins"`

### JWT for SignalR
JWT bearer config reads token from query string for:
- `/hubs/orders?access_token=<JWT>`

### Server Event
When a new order is created, API pushes:
- Event: `NewOrderCreated`
- Payload: `NewOrderNotificationDto`

---

## Error Handling

A global `ExceptionMiddleware` exists that:
- catches unhandled exceptions
- returns JSON with `statusCode`, `message`, and `detail` (for 500)

If not already registered, add to pipeline:
```csharp
app.UseMiddleware<ExceptionMiddleware>();
```

---

## Notes / Known Considerations

- `app.UseHttpsRedirection()` is currently disabled for SignalR testing.
- Refresh token end-date uses `AddSeconds(...)` in `UpdateRefreshTokenAsync`; confirm intended lifetime.
- `RolePermissionFilter` currently injects `UserService` concrete type; prefer `IUserService` to reduce coupling.
- `UseForwardedHeaders` is enabled (useful behind reverse proxies).

---

## Contributors

- **Hakan Onay**
- **Mehmet Çakmak**
