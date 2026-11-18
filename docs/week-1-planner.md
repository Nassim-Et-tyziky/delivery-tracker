# Week 1 planner · 16–22 Nov 2026

## Goal of week 1

By Sunday, Habib logs in on his phone against Nassim's real API and sees a list of test delivery tasks. That one demo proves the setup, the contract and the auth all work.

**Done means**

- One GitHub repo with `/api`, `/web`, `/mobile`, a README and a project board
- Database model and OpenAPI contract v1 agreed and committed
- API runs with Docker (Postgres), has login + JWT and task endpoints with test data
- Flutter app has login, task list and task detail screens connected to the API

**Rhythm:** about 2–3 hours a day each (about 18–19 hours for the week, shared sessions included). A 10-minute check-in on Discord every evening: what I did, what I'll do tomorrow, what's blocking me.

## Monday 16 Nov · Kickoff and setup

**Together (1 h)**

- [x] Kickoff call: confirm the MVP scope, pick the dashboard framework (React chosen), agree on working hours and the evening check-in (45 min)
- [ ] Create the Discord server and the GitHub Projects board (15 min)

**Nassim (1.5 h)**

- [x] Create the repo with `/api`, `/web`, `/mobile`, `.gitignore` and branch rules: `main` protected, work on feature branches, merge by pull request (1 h)
- [ ] Add week 1 tasks as issues on the board (30 min)

**Habib (1.5 h)**

- [ ] Install Flutter, Android Studio and an emulator; run `flutter doctor` until it's clean (1 h)
- [ ] Create the Flutter project in `/mobile` and open the first pull request (30 min)

## Tuesday 17 Nov · Data model and project structure

**Together (1 h)**

- [ ] Design the database on dbdiagram.io: User, Driver, DeliveryTask, TaskStatusHistory, LocationPoint, Proof (1 h)

**Nassim (2 h)**

- [ ] Create the .NET solution with Clean Architecture: Domain, Application, Infrastructure, Api projects (1.5 h)
- [ ] Add Swagger, Serilog and a `/health` endpoint (30 min)

**Habib (2 h)**

- [ ] Set up the folder structure (features/, core/, shared/) and add Riverpod, go_router and Dio (1.5 h)
- [ ] Create the app theme: colors, fonts, base widgets (30 min)

## Wednesday 18 Nov · API contract v1

**Together (1.5 h)**

- [ ] Write OpenAPI contract v1 in `contracts/openapi.yaml`: login, refresh, `GET /me/tasks`, `GET /tasks/{id}`, `PATCH /tasks/{id}/status`, with example JSON for each (1.5 h)

**Nassim (2 h)**

- [ ] Write `docker-compose.yml` with PostgreSQL (and pgAdmin) (45 min)
- [ ] Add EF Core entities from the data model and run the first migration (1 h 15 min)

**Habib (1.5 h)**

- [ ] Sketch the driver screens (login, task list, task detail) in Figma or Excalidraw and share them with Nassim (1.5 h)

## Thursday 19 Nov · Login

**Nassim (3 h)**

- [ ] Add ASP.NET Core Identity with Admin, Dispatcher and Driver roles (1 h)
- [ ] Build `POST /auth/login` and `POST /auth/refresh` with JWT; enable the Authorize button in Swagger (2 h)

**Habib (2.5 h)**

- [ ] Build the login screen with form validation (1.5 h)
- [ ] Add a fake auth service that returns the contract's example JSON, and save the token securely with flutter_secure_storage (1 h)

## Friday 20 Nov · Tasks endpoints and screens

**Nassim (3 h)**

- [ ] Seed test drivers and tasks with Bogus (45 min)
- [ ] Build `GET /me/tasks`, `GET /tasks/{id}` and `PATCH /tasks/{id}/status` with status rules (2 h 15 min)

**Habib (3 h)**

- [ ] Build the task list screen with status chips and pull-to-refresh, using mock data (1.5 h)
- [ ] Build the task detail screen with customer info, addresses and status buttons (1.5 h)

## Saturday 21 Nov · Connect the two sides

**Nassim (2.5 h)**

- [ ] Enable CORS, share a Bruno or Postman collection, and make the API reachable from Habib's phone (same network, or a free ngrok / Cloudflare tunnel) (1.5 h)
- [ ] Create the React + Vite project in `/web` with a login page (1 h)

**Habib (2.5 h)**

- [ ] Replace the mocks with real Dio calls to Nassim's API, with an interceptor that adds the token (2 h)
- [ ] Handle loading, error and expired-token states (30 min)

## Sunday 22 Nov · Demo and plan week 2

**Together (1.5 h)**

- [ ] Demo: Habib logs in on his phone and sees the seeded tasks from the real API; change a status and check it in the database (30 min)
- [ ] Retro: what went well, what slowed us down (20 min)
- [ ] Plan week 2 on the board: tasks CRUD on the web dashboard, driver status updates, start of SignalR (40 min)

**Week total:** Nassim about 19 h, Habib about 18 h, shared sessions included.
