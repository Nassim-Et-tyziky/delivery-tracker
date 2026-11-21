# Data model v1 (draft for review)

How the API stores data. The clients never see these tables directly, only the JSON in
[`openapi.yaml`](openapi.yaml), but the two must stay consistent.

**To see it as a diagram:** copy the DBML block below and paste it into https://dbdiagram.io/d.

## Tables

| Table | What it is | Week 1? |
| --- | --- | --- |
| `users` | Everyone who logs in (admin, dispatcher, driver). Managed by ASP.NET Core Identity, which adds its own tables (`roles`, `user_roles`, …). | ✅ |
| `drivers` | Extra info for users with the Driver role (1-to-1 with `users`). | ✅ |
| `delivery_tasks` | One delivery: customer, pickup, drop-off, status. | ✅ |
| `task_status_history` | One row per status change, used for the timeline and reports. | ✅ |
| `refresh_tokens` | Refresh tokens for login (stored hashed, never in plain text). | ✅ |
| `location_points` | GPS points sent by drivers. | Later (sprint 3) |
| `proofs` | Photo, signature and note for a delivered task. | Later (sprint 4) |

## DBML

```dbml
Enum task_status {
  Created
  Assigned
  Accepted
  PickedUp
  InTransit
  Delivered
  Failed
  Cancelled
}

Table users {
  id uuid [pk]
  email varchar(256) [unique, not null]
  full_name varchar(200) [not null]
  phone_number varchar(20)
  password_hash text [not null, note: 'Identity manages this']
  is_active boolean [not null, default: true]
  created_at timestamptz [not null]
  Note: 'ASP.NET Core Identity user. Role (Admin / Dispatcher / Driver) is stored in the Identity role tables.'
}

Table drivers {
  id uuid [pk]
  user_id uuid [unique, not null, ref: - users.id]
  vehicle_type varchar(50) [note: 'Motorbike, Car, Van, Bicycle']
  is_available boolean [not null, default: false, note: 'Online / offline switch in the app']
  created_at timestamptz [not null]
}

Table delivery_tasks {
  id uuid [pk]
  reference varchar(20) [unique, not null, note: 'DT-1042, for phone calls']
  status task_status [not null, default: 'Created']
  customer_name varchar(200) [not null]
  customer_phone varchar(20)
  pickup_address varchar(500) [not null]
  pickup_latitude double
  pickup_longitude double
  dropoff_address varchar(500) [not null]
  dropoff_latitude double
  dropoff_longitude double
  time_window_start timestamptz
  time_window_end timestamptz
  notes text
  driver_id uuid [ref: > drivers.id, note: 'null until assigned']
  created_by_user_id uuid [not null, ref: > users.id]
  created_at timestamptz [not null]
  updated_at timestamptz [not null]

  indexes {
    (driver_id, status)
    status
  }
}

Table task_status_history {
  id uuid [pk]
  task_id uuid [not null, ref: > delivery_tasks.id]
  from_status task_status [note: 'null for the first row (Created)']
  to_status task_status [not null]
  reason varchar(500) [note: 'required when to_status = Failed']
  changed_by_user_id uuid [not null, ref: > users.id]
  changed_at timestamptz [not null]

  indexes {
    (task_id, changed_at)
  }
}

Table refresh_tokens {
  id uuid [pk]
  user_id uuid [not null, ref: > users.id]
  token_hash varchar(128) [unique, not null]
  expires_at timestamptz [not null]
  created_at timestamptz [not null]
  revoked_at timestamptz [note: 'set when used (rotation) or on logout']
  replaced_by_token_id uuid [ref: - refresh_tokens.id]
}

// ---- Later sprints ----

Table location_points {
  id bigint [pk, increment]
  driver_id uuid [not null, ref: > drivers.id]
  latitude double [not null]
  longitude double [not null]
  accuracy_meters real
  recorded_at timestamptz [not null, note: 'time on the phone']
  received_at timestamptz [not null, note: 'time on the server']

  indexes {
    (driver_id, recorded_at)
  }
}

Table proofs {
  id uuid [pk]
  task_id uuid [unique, not null, ref: - delivery_tasks.id]
  photo_url text
  signature_url text
  note text
  created_at timestamptz [not null]
}
```

## Design choices (to discuss)

1. **UUIDs for ids**, not 1, 2, 3: they can be created on the phone while offline (useful later
   for Drift sync), and they don't reveal how many tasks a business has. `reference` gives humans a
   short code instead.
2. **Addresses are columns on the task**, not a separate `addresses` table: simpler for the MVP.
   Latitude/longitude become a PostGIS `geography` column when we build "nearest driver".
3. **`status` is stored on the task *and* every change goes into `task_status_history`**: the task
   row answers "where is it now?" quickly, the history answers "what happened and when?".
4. **Times are `timestamptz` (UTC)**, matching the ISO 8601 UTC dates in the API.
5. **`drivers` is separate from `users`**: dispatchers and admins don't need vehicle or
   availability fields.

## Open questions for the review

- **Reject a task:** the plan says a driver can "accept or reject". Should rejecting send the task
  back to `Created` (unassigned), with a reason? Not in v1 of the contract yet.
- **`GET /me/tasks`:** only open tasks (current draft), or also today's finished ones for the
  history screen?
- **Driver can go back a step?** (e.g. `InTransit` → `PickedUp` after a wrong tap). Draft says no.
- **Multi-company:** v2 in the plan. Adding a `company_id` later is a migration on every table;
  fine to postpone, but worth knowing.
