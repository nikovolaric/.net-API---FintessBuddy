# DevOps

Local Postgres for FitnessBuddy API via Docker.

## Start

```bash
cd devops
cp .env.example .env   # optional: override defaults
docker compose up -d
```

Postgres listens on `localhost:5432`. Default creds: `fitnessbuddy` / `fitnessbuddy`, db `fitnessbuddy`.

## Stop

```bash
docker compose down        # keep data
docker compose down -v      # wipe data volume
```

## API connection

`appsettings.json` `ConnectionStrings:DefaultConnection` points at this container:

```
Host=localhost;Port=5432;Database=fitnessbuddy;Username=fitnessbuddy;Password=fitnessbuddy
```

Override per-env via `ConnectionStrings__DefaultConnection` env var (never commit real prod creds).

## Run migrations

```bash
dotnet ef database update
```
