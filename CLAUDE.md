# Delivery Tracker — project context for Claude Code

## What this project is
A delivery tracker MVP built by two people as a learning project.
A dispatcher creates delivery tasks on the web, a driver completes them on a phone,
and both see progress live. Target users: small businesses (shops, pharmacies,
restaurants, couriers) that manage drivers by phone calls and WhatsApp today.

Learning new tech is a main goal: when suggesting solutions, prefer the modern,
professional approach and explain *why*, briefly.

Full plan: `docs/project-plan.md` · Current week: `docs/week-1-planner.md`

## Team and ownership
| Area | Folder | Owner |
| --- | --- | --- |
| ASP.NET Core API | `/api` | Nassim (`@Nassim-Et-tyziky`) |
| React dashboard | `/web` | Nassim |
| Flutter driver app | `/mobile` | Habib (`@HABIB01288`) |
| API contract (OpenAPI) | `/contracts` | Both — shared |

Never change files outside the folder of the person you are helping, except
`/contracts` (see rules below). If a task needs a change in another person's
folder, say so instead of doing it.

## Architecture
- The two apps never talk to each other. Everything goes through the API.
- The API is the single source of truth: data, business rules, real-time hub.
- `contracts/openapi.yaml` is the agreement between web/mobile and the API.

## Tech stack (decided)
**API (`/api`)**
- ASP.NET Core Web API (.NET 9/10), Clean Architecture: `Domain`, `Application`, `Infrastructure`, `Api`
- CQRS with MediatR, FluentValidation
- PostgreSQL + EF Core (later PostGIS + NetTopologySuite for "nearest driver")
- ASP.NET Core Identity + JWT access/refresh tokens; roles: Admin, Dispatcher, Driver
- SignalR hub `/hubs/tracking`: `LocationUpdated`, `TaskStatusChanged`, `TaskAssigned`
- Hangfire (background jobs), Redis (cache + SignalR backplane) — later sprints
- Blob storage: MinIO locally, Azure Blob or S3 in the cloud — later
- Firebase Cloud Messaging for push — later
- Serilog (+ OpenTelemetry later); tests with xUnit, Testcontainers, Bogus
- Docker Compose for local Postgres (+ pgAdmin)

**Web (`/web`)**
- React + TypeScript with Vite
- shadcn/ui + Tailwind, TanStack Query for API calls
- Leaflet + OpenStreetMap for maps, SignalR JS client for live updates

**Mobile (`/mobile`)**
- Flutter, Riverpod, go_router, Dio (+ token interceptor)
- flutter_secure_storage for tokens; later geolocator (background GPS),
  Drift for offline, signalr_netcore, firebase_messaging, image_picker, signature

## Domain
Entities: User, Driver, DeliveryTask, TaskStatusHistory, LocationPoint, Proof.

Task statuses: `Created → Assigned → Accepted → PickedUp → InTransit → Delivered`,
with `Failed` and `Cancelled` as exits. Enforce allowed transitions in the API.

Main endpoints (v1):
- `POST /auth/login`, `POST /auth/refresh`
- `GET /me/tasks`, `GET /tasks/{id}`, `PATCH /tasks/{id}/status` (driver)
- `POST /tasks`, `GET /tasks?status=&driverId=`, `POST /tasks/{id}/assign` (dispatcher)
- `GET/POST /drivers`, `PATCH /drivers/{id}/availability`
- later: `POST /me/location`, `POST /tasks/{id}/proof`, `GET /reports/daily`

## Git workflow (enforced on GitHub)
- `main` is protected: no direct pushes. Every change goes through a pull request.
- Branch names by area: `api/...`, `web/...`, `mobile/...`, `chore/...`, `contracts/...`
- Small, short-lived branches (1–2 days). Each person merges his own PRs.
- Any change to `/contracts` needs the other person's approval before merge.
  Change the contract first (its own PR), then the code.
- Commit messages: Conventional Commits (`feat:`, `fix:`, `chore:`, `docs:`, `refactor:`, `test:`).
- Never commit secrets: `appsettings.Development.json` and `.env` are git-ignored;
  commit `appsettings.example.json` / `.env.example` with placeholders instead.

## How to help
- Developer environment: Windows + Git Bash.
- Explain new concepts briefly when you introduce them (this is a learning project).
- Keep changes small and focused on the current task in `docs/week-1-planner.md`.
- After code changes, say how to run or test them.
