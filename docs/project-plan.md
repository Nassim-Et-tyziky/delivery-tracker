# Delivery Tracker MVP — Project Plan

## Overview

We are building a delivery tracker: a dispatcher creates delivery tasks on the web, a driver completes them on a phone, and both see progress live. It is aimed at small businesses (shops, pharmacies, restaurants, couriers) that still manage drivers by phone calls and WhatsApp.

| User | Uses | Main job |
| --- | --- | --- |
| Admin / dispatcher | Web app | Create tasks, assign drivers, watch the live map, read reports |
| Driver | Mobile app | Receive tasks, navigate, update status, capture proof of delivery |
| End customer (v2) | Public tracking link | See where their order is |

**Team split:** Nassim builds the ASP.NET Core API and the React web dashboard; Habib builds the Flutter driver app. The API contract (OpenAPI/Swagger) is the agreement between us.

## What the API does (ASP.NET Core)

The API is the single source of truth: it owns the data, the business rules and the real-time hub, and both apps only talk to it.

**Responsibilities**

- Authentication and roles (Admin, Dispatcher, Driver) with JWT access and refresh tokens
- Task lifecycle rules: which status can follow which, who may change it
- Storing driver locations and pushing them live to the dashboard
- Accepting proof-of-delivery uploads (photo, signature) and storing them in blob storage
- Background jobs: overdue alerts, auto-assignment, daily report generation
- Sending push notifications to drivers

**Main endpoint groups**

| Group | Example endpoints | Used by |
| --- | --- | --- |
| Auth | `POST /auth/login`, `POST /auth/refresh` | Web, Mobile |
| Drivers | `GET /drivers`, `POST /drivers`, `PATCH /drivers/{id}/availability` | Web, Mobile |
| Tasks | `POST /tasks`, `GET /tasks?status=&driverId=`, `POST /tasks/{id}/assign` | Web |
| Driver tasks | `GET /me/tasks`, `PATCH /tasks/{id}/status` | Mobile |
| Locations | `POST /me/location` (batch of GPS points) | Mobile |
| Proof | `POST /tasks/{id}/proof` (photo, signature, note) | Mobile |
| Reports | `GET /reports/daily`, `GET /reports/drivers` | Web |
| Real-time hub | `/hubs/tracking`: `LocationUpdated`, `TaskStatusChanged`, `TaskAssigned` | Web, Mobile |

**Task statuses:** Created → Assigned → Accepted → PickedUp → InTransit → Delivered, with Failed and Cancelled as exits.

## What the web app does (dispatcher dashboard)

The web app is the control room: the dispatcher plans the day and watches it happen live.

- **Login and roles:** admin manages users; dispatcher manages tasks
- **Drivers:** add, edit and deactivate drivers; see who is online, busy or offline
- **Tasks:** create a task (customer name, phone, pickup and drop-off address on a map, time window, notes); edit or cancel it
- **Assignment:** assign a task to a driver by hand, or suggest the nearest available driver
- **Live map:** every driver's position and task status updating in real time
- **Task board:** list or kanban view filtered by status, driver and date
- **Task details:** status timeline, route taken, proof-of-delivery photo and signature
- **Alerts:** a task late or failed shows up instantly on screen
- **Reports:** deliveries per day, success rate, average delivery time, per-driver stats

## What the mobile app does (Flutter driver app)

The mobile app is the driver's tool for the day: see the tasks, do them, prove them.

- **Login** and an online / offline switch (go on shift, go off shift)
- **My tasks:** today's list ordered by time window, with a push notification when a new task is assigned
- **Accept or reject** a newly assigned task
- **Task details:** customer info, tap-to-call, addresses on a map, open in Google Maps or Waze for navigation
- **Status updates:** one tap for Picked up, In transit, Delivered or Failed (with a reason)
- **Proof of delivery:** take a photo, capture the customer's signature, add a note
- **Background GPS:** send the driver's location every few seconds while on shift, even when the app is in the background
- **Offline mode:** keep tasks and pending status updates locally, sync when the connection comes back
- **History:** completed deliveries and simple daily stats for the driver

## Core flow: life of a delivery task

The two apps never talk to each other directly: every action goes to the API, which saves it and pushes it to the other side in real time.

1. Web: dispatcher creates a task → API saves it (status Created)
2. Web: dispatcher assigns a driver → API marks Assigned, pushes via FCM + SignalR → Mobile: driver gets an alert, accepts or rejects
3. Mobile: pick up, start trip → API validates and saves the status, broadcasts it → Web: task board updates live
4. Mobile: GPS in the background → API stores the points, broadcasts LocationUpdated → Web: driver moves on the map
5. Mobile: deliver + proof (photo, signature, note) → API saves proof to storage, status Delivered → Web: task closed, counted in the daily report

If the driver is offline at any step, the mobile app queues the update and the API receives it when the connection returns.

## Technologies

**API (ASP.NET Core)**

| Need | Choice | What we learn |
| --- | --- | --- |
| Framework | ASP.NET Core Web API (.NET 9/10) | Modern .NET API design |
| Architecture | Clean Architecture + CQRS with MediatR, FluentValidation | How professional .NET projects are structured |
| Database | PostgreSQL + EF Core | Migrations, relations, query performance |
| Geo queries | PostGIS + NetTopologySuite | "Nearest driver", distances, areas |
| Auth | ASP.NET Core Identity + JWT + refresh tokens | Secure auth for two different clients |
| Real-time | SignalR | Live map and live status updates |
| Background jobs | Hangfire (or Quartz.NET) | Scheduled and delayed work |
| Cache / scale | Redis (cache + SignalR backplane) | Caching and running several API instances |
| File storage | Azure Blob Storage or AWS S3 (MinIO locally) | Uploads with pre-signed URLs |
| Push notifications | Firebase Cloud Messaging (FCM) | Server-side push to phones |
| Logging / tracing | Serilog + OpenTelemetry | Seeing what happens in production |
| Testing | xUnit, Testcontainers, Bogus | Integration tests against a real database |

**Web dashboard**

| Need | Choice | What we learn |
| --- | --- | --- |
| UI framework | React + TypeScript (Vite) | A mainstream JS framework |
| UI components | shadcn/ui + Tailwind, TanStack Query for API calls | Building dashboards fast |
| Maps | Leaflet + OpenStreetMap (free) or Mapbox | Markers, live positions, routes |
| Charts | ApexCharts or Chart.js | Report pages |
| Real-time client | SignalR JS client | Consuming the hub |

**Mobile app (Flutter)**

| Need | Choice | What we learn |
| --- | --- | --- |
| State management | Riverpod (or Bloc) | Scalable app state |
| HTTP | Dio + Retrofit, generated from the OpenAPI spec | Typed API clients |
| Maps | google_maps_flutter or flutter_map | Maps on mobile |
| Location | geolocator + a background location package | Background GPS tracking |
| Offline storage | Drift (SQLite) or Isar | Offline-first and sync |
| Real-time | signalr_netcore | Live task assignment |
| Push | firebase_messaging | Receiving notifications |
| Camera / signature | image_picker, signature | Native device features |
| Routing | go_router | Navigation structure |

**Shared tooling**

| Need | Choice | What we learn |
| --- | --- | --- |
| Containers | Docker + Docker Compose (API, Postgres, Redis, MinIO) | One-command local setup |
| CI/CD | GitHub Actions: build, test, deploy | Automated pipelines |
| Hosting | Azure App Service, Railway or a small VPS | Real deployment |
| API contract | Swagger / OpenAPI, tested with Bruno or Postman | Working in parallel without blocking |
| Project management | GitHub Projects | Planning as a team |

## MVP scope and roadmap

Ship the core loop first (create → assign → deliver → proof), then add one new technology per sprint so each one is learned properly.

| In the MVP | Later (v2) |
| --- | --- |
| Login with roles | Public tracking link for end customers (SMS / WhatsApp) |
| Drivers and tasks CRUD | Route optimization for several stops |
| Manual assignment + nearest-driver suggestion | Fully automatic assignment |
| Status updates + proof photo and signature | Payments / cash-on-delivery reconciliation |
| Live map with SignalR | Multi-company (multi-tenant) support |
| Push notifications to drivers | Customer ratings and driver performance scores |
| Basic daily report | Advanced analytics and exports |

**Suggested 2-week sprints (about 12 weeks)**

1. **Foundations:** agree on the OpenAPI contract, set up repos, Docker Compose, database schema, auth. Mobile builds screens against mock data.
2. **Core tasks:** tasks and drivers CRUD on the API and web; driver task list and status updates on mobile.
3. **Real-time:** SignalR hub, live map on the web, background GPS on mobile.
4. **Proof and notifications:** blob storage uploads, photo and signature, FCM push.
5. **Background jobs and offline:** Hangfire alerts and reports; offline mode and sync on mobile.
6. **Ship it:** tests, Serilog + OpenTelemetry, GitHub Actions, deploy to the cloud, publish a test build of the app.
