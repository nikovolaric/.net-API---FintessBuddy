# MyFitnessBuddy API

Backend API for a fitness tracking application built with ASP.NET Core.
Focuses on secure API design, authentication, and relational data modeling.

---

## Stack

- .NET 10 Web API
- Entity Framework Core (code-first)
- PostgreSQL (local via Docker)
- JWT auth over HttpOnly cookies
- Swagger / OpenAPI (dev only)

---

## What this project demonstrates

- Secure JWT authentication and role-based authorization
- Clean REST API design with a service layer + `ServiceResult` pattern
- EF Core code-first workflow with migrations
- Protected endpoints and policy-based access control
- Swagger-driven API documentation

---

## Getting started

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/)
- [Docker](https://www.docker.com/) (for PostgreSQL)

### 1. Start the database

```bash
cd devops
cp .env.example .env   # optional: override defaults
docker compose up -d
```

Postgres listens on `localhost:5432`. See [`devops/README.md`](devops/README.md) for details.

### 2. Configure secrets

The API needs a JWT signing key. In development it is read from `launchSettings.json`.
For any other environment set it via env var:

```bash
export JWT_SECRET_KEY="your-long-random-secret"
```

Override the DB connection with `ConnectionStrings__DefaultConnection` if not using the default Docker setup.

### 3. Apply migrations

```bash
dotnet ef database update
```

### 4. Run

```bash
dotnet run
```

- HTTP: `http://localhost:5034`
- Swagger UI (dev): `http://localhost:5034/swagger`

---

## Endpoints

### Auth — `/api/auth`

| Method | Route     | Auth | Description                     |
| ------ | --------- | ---- | ------------------------------- |
| POST   | `/signup` | —    | Register a user                 |
| POST   | `/login`  | —    | Log in, sets `access_token` cookie |
| POST   | `/logout` | User | Clear auth cookie               |

### Exercises — `/api/exercises`

| Method | Route         | Auth  | Description                          |
| ------ | ------------- | ----- | ------------------------------------ |
| GET    | `/`           | User  | List exercises (`?search=` optional) |
| GET    | `/{id}`       | User  | Get one exercise                     |
| POST   | `/add`        | Admin | Create an exercise                   |
| PUT    | `/{id}`       | Admin | Update an exercise                   |
| DELETE | `/{id}`       | Admin | Delete an exercise                   |

### Workouts — `/api/workouts`

| Method | Route                | Auth | Description                     |
| ------ | -------------------- | ---- | ------------------------------- |
| POST   | `/`                  | User | Create a workout                |
| PUT    | `/{id}/addexercise`  | User | Add an exercise to a workout    |
| DELETE | `/{id}`              | User | Delete a workout                |
| GET    | `/getmy`             | User | List the current user's workouts |

Auth is carried by the `access_token` HttpOnly cookie set at login. Admin routes require the `admin` role.

---

## Scope

Backend only. No frontend included.
