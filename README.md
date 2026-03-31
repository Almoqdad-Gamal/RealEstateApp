# 🏠 Real Estate API

<div align="center">

![.NET](https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet)
![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp)
![SQL Server](https://img.shields.io/badge/SQL_Server-CC2927?style=for-the-badge&logo=microsoftsqlserver)
![Redis](https://img.shields.io/badge/Redis-DC382D?style=for-the-badge&logo=redis)
![Docker](https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker)
![SignalR](https://img.shields.io/badge/SignalR-512BD4?style=for-the-badge&logo=dotnet)

A **production-ready** RESTful API for a Real Estate platform, built with Clean Architecture, CQRS, and modern .NET best practices.

[Features](#-features) • [Architecture](#-architecture) • [Getting Started](#-getting-started) • [API Docs](#-api-documentation) • [Tech Stack](#-tech-stack)

</div>

---

## ✨ Features

### 🔐 Authentication & Security
- **JWT Authentication** with Access Token (15 min) + Refresh Token (7 days)
- **Role-Based Authorization** — Admin, Owner, Client
- **Forgot/Reset Password** via secure email link
- **Rate Limiting** — 5 req/min on auth endpoints, 100 req/min on API
- **CORS** policy for frontend origins

### 🏠 Properties
- Full CRUD with ownership validation
- **Advanced Search & Filtering** — city, price, area, bedrooms, amenities
- **Sorting** — by price, area, bedrooms (asc/desc)
- **Pagination** with configurable page size (max 50)
- **Image Upload** via Cloudinary with auto-optimization

### 📅 Bookings
- Create, view, cancel bookings
- Time slot availability check — no double bookings
- Status management (Pending → Confirmed → Completed)
- Ownership-based access control

### ⭐ Reviews
- One review per user per property
- Rating system (1-5 stars)
- Average rating calculation per property

### ❤️ Favorites / Wishlist
- Add/remove properties to favorites
- Check if a property is favorited
- Per-user favorites list

### 🔔 Real-time Notifications (SignalR)
- Instant notification to Owner on new Booking
- Instant notification to Client on Booking status update
- Instant notification to Owner on new Review
- Persistent notification history
- Unread count tracking

### 📊 Admin Dashboard
- Total users, properties, bookings, reviews, favorites
- Monthly statistics
- Bookings breakdown by status
- Properties breakdown by type, city, listing type
- Top-rated properties

### ⚡ Performance
- **Redis Caching** — properties, bookings, reviews, notifications
- **Cache Invalidation** on all write operations
- **Server-Side Pagination** at database level

### 🔍 Observability
- **Serilog** structured logging — Console + daily rolling files
- **Health Checks** — SQL Server + Redis
- Request logging middleware

---

## 🏗 Architecture

```
RealEstateApp/
├── RealEstateApp.API/              # Controllers, Hubs, Middleware, Extensions
│   ├── Controllers/
│   ├── Hubs/                       # SignalR NotificationHub
│   ├── Middleware/                 # GlobalExceptionHandlerMiddleware
│   ├── Extensions/                 # Auth, Cache, CORS, DB, RateLimit, Health
│   └── Services/                   # NotificationService
│
├── RealEstateApp.Application/      # Business Logic (CQRS + MediatR)
│   ├── Features/
│   │   ├── Users/
│   │   ├── Properties/
│   │   ├── Bookings/
│   │   ├── Reviews/
│   │   ├── Favorites/
│   │   ├── Notifications/
│   │   └── Admin/
│   ├── Interfaces/                 # Repository & Service Interfaces
│   ├── DTOs/
│   ├── Behaviors/                  # ValidationBehavior (Pipeline)
│   └── Exceptions/
│
├── RealEstateApp.Domain/           # Entities & Enums
│   ├── Entities/
│   └── Enums/
│
└── RealEstateApp.Infrastructure/   # Data Access & External Services
    ├── Data/                       # DbContext, Seeder, Migrations
    ├── Repositories/
    ├── Configurations/
    └── Services/                   # JWT, Email, Cloudinary, Redis, Notifications
```

### Design Patterns Used
- **Clean Architecture** — strict layer separation
- **CQRS** — Commands and Queries separated via MediatR
- **Repository Pattern** + **Unit of Work**
- **Pipeline Behaviors** — validation runs automatically before every command
- **Soft Delete** — data is never permanently deleted
- **Global Exception Handling** — consistent error responses

---

## 🚀 Getting Started

### Prerequisites
- [Docker Desktop](https://www.docker.com/products/docker-desktop)
- That's it!

### Setup

**1. Clone the repository**
```bash
git clone https://github.com/Almoqdad-Gamal/RealEstateApp.git
cd RealEstateApp
```

**2. Create your environment file**
```bash
cp .env.example .env
```

**3. Fill in your values in `.env`**
```env
SA_PASSWORD=YourStrong@Password123
JWT_SECRET_KEY=your-secret-key-min-32-characters
CLOUDINARY_CLOUD_NAME=your-cloud-name
CLOUDINARY_API_KEY=your-api-key
CLOUDINARY_API_SECRET=your-api-secret
EMAIL_SENDER=your-email@gmail.com
EMAIL_PASSWORD=your-gmail-app-password
ADMIN_EMAIL=admin@realestate.com
ADMIN_PASSWORD=Admin@12345
ADMIN_PHONE=+201000000000
```

**4. Run**
```bash
docker-compose up --build
```

The API will automatically:
- Start SQL Server and Redis
- Run all database migrations
- Seed sample data

---

## 📖 API Documentation

After running, open:

| URL | Description |
|-----|-------------|
| `http://localhost:8080/scalar/v1` | Interactive API Docs |
| `http://localhost:8080/health` | Health Check |

---

## 🧪 Test Accounts

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@realestate.com | From `.env` |
| Owner | ahmed.owner@realestate.com | Owner@12345 |
| Owner | sara.owner@realestate.com | Owner@12345 |
| Client | omar.client@realestate.com | Client@12345 |
| Client | nour.client@realestate.com | Client@12345 |
| Client | youssef.client@realestate.com | Client@12345 |

---

## 📡 API Endpoints

### Auth
```
POST /api/auth/register
POST /api/auth/login
POST /api/auth/refresh-token
POST /api/auth/forgot-password
POST /api/auth/reset-password
```

### Properties
```
GET    /api/properties                     # All available (paginated)
GET    /api/properties/{id}                # Single property
GET    /api/properties/search              # Advanced search & filter
GET    /api/properties/my-properties       # Owner's properties
GET    /api/properties/owner/{ownerId}     # Admin: properties by owner
POST   /api/properties                     # Create (Owner/Admin)
PUT    /api/properties/{id}                # Update (Owner/Admin)
DELETE /api/properties/{id}                # Delete (Owner/Admin)
POST   /api/properties/{id}/images         # Upload image (Owner/Admin)
```

### Bookings
```
GET    /api/bookings/my-bookings           # User's bookings
GET    /api/bookings/{id}                  # Single booking
POST   /api/bookings                       # Create (Client)
PUT    /api/bookings/{id}/status           # Update status (Owner/Admin)
DELETE /api/bookings/{id}                  # Cancel
```

### Reviews
```
GET    /api/reviews/property/{propertyId}  # Property reviews
POST   /api/reviews                        # Create (Client)
DELETE /api/reviews/{id}                   # Delete (Client)
```

### Favorites
```
GET    /api/favorites                      # My favorites
GET    /api/favorites/{propertyId}/check   # Is favorited?
POST   /api/favorites/{propertyId}         # Add to favorites
DELETE /api/favorites/{propertyId}         # Remove from favorites
```

### Notifications
```
GET    /api/notifications                  # My notifications
GET    /api/notifications/unread-count     # Unread count
PUT    /api/notifications/mark-all-read    # Mark all as read
WS     /hubs/notifications                 # SignalR Hub
```

### Users
```
GET    /api/users/profile                  # My profile
PUT    /api/users/profile                  # Update profile
POST   /api/users/admins                   # Create admin (Admin only)
```

### Admin
```
GET    /api/admin/stats                    # Dashboard statistics
```

---

## 🛠 Tech Stack

| Category | Technology |
|----------|-----------|
| Framework | ASP.NET Core 10 |
| Language | C# |
| Database | SQL Server 2022 |
| ORM | Entity Framework Core 10 |
| Cache | Redis (via StackExchange.Redis) |
| Real-time | SignalR |
| Auth | JWT Bearer Tokens |
| Logging | Serilog |
| Validation | FluentValidation |
| Mediator | MediatR 14 |
| Image Storage | Cloudinary |
| Email | SMTP (Gmail) |
| Containerization | Docker + Docker Compose |
| API Docs | Scalar |
| Password Hashing | BCrypt |

---

## 🔒 Security

- Secrets managed via `.env` file — never committed to source control
- Passwords hashed with BCrypt
- JWT tokens short-lived (15 min) with Refresh Token rotation
- Role-based authorization on all sensitive endpoints
- Resource ownership validation — users can only modify their own data
- Rate limiting on authentication endpoints to prevent brute force
- Global exception handler — never exposes internal errors

---

## 📁 Project Structure Highlights

```
✅ Clean Architecture          — strict dependency rules
✅ CQRS with MediatR           — commands and queries separated
✅ Repository + Unit of Work   — abstracted data access
✅ FluentValidation Pipeline   — automatic request validation
✅ Global Exception Handling   — consistent error format
✅ Soft Delete                 — data retention
✅ Auto Migrations             — database updates on startup
✅ Database Seeding            — ready-to-use sample data
✅ Redis Caching               — with proper invalidation
✅ Structured Logging          — with daily file rotation
✅ Health Checks               — for DB and Cache
✅ Rate Limiting               — per endpoint control
✅ CORS                        — configurable origins
✅ Real-time Notifications     — via SignalR
✅ Docker                      — one command to run everything
```

---

## 📄 License

This project is for educational and portfolio purposes.

---

<div align="center">
Built with ❤️ using Clean Architecture & .NET 10
</div>
