# PondLite

PondLite is a gentle, private relationship app for couples. The backend/domain
term is `Relationship`; the user-facing term is `Pond`. People connected to the
same Pond are `RelationshipMember`s in the backend and PondMates in the product
language.

The app is designed to help partners stay connected through small, low-pressure
moments: daily check-ins, shared frog states, Ribbits, bug gifts, optional
discussion prompts, and eventually a Smart Connection Assistant.

This is also a learning project focused on ASP.NET Core, C#, Entity Framework
Core, PostgreSQL, layered backend architecture, and Postman-based API testing.

---

## Current Status

Backend Split I is in progress.

Completed:

- Phase 1: Authentication Foundation
- Phase 2: Relationship/Pond Foundation
- Phase 3: Frog Foundation
- Phase 4: Daily Check-In

Next planned phase:

- Phase 5: Ribbits / Gentle Reminders

There is no frontend yet.

---

## Tech Stack

- C#
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Npgsql EF Core provider
- Manual bearer-token authentication for early development
- Repository/service/controller architecture
- Postman for manual API testing

---

## Architecture

The backend follows this flow:

```text
Controller
    -> Service
    -> Repository
    -> PondLiteDbContext
    -> PostgreSQL
```

Controllers receive HTTP requests and stay thin. Services hold business rules.
Repositories handle database access. DTOs shape request and response data, and
EF entities are not returned directly from controllers.

Privacy rule:

```text
Authentication answers: Who is this user?
RelationshipMember answers: What Pond can this user access?
```

Relationship/Pond-owned data must be checked through membership before it is
returned, created, updated, or deleted.

---

## Implemented Models

### UserAccount

Represents a registered user.

- `AccountId`
- `Username`
- `Email`
- `PasswordHash`
- `CreatedAt`
- `RelationshipMembers`

### AuthToken

Represents an active or expired login token.

- `TokenId`
- `UserAccountId`
- `TokenValue`
- `CreatedAt`
- `ExpiresAt`
- `IsActive`
- `UserAccount`

### Relationship

Represents the private backend data boundary for a couple. In the UI this is a
Pond.

- `RelationshipId`
- `Name`
- `CreatedAt`
- `UpdatedAt`
- `Members`

### RelationshipMember

Connects a user account to a Relationship/Pond.

- `RelationshipMemberId`
- `RelationshipId`
- `UserAccountId`
- `DisplayName`
- `Role`
- `JoinedAt`
- `Relationship`
- `UserAccount`
- `Frog`

Current v1 behavior supports one active Pond per user.

### Frog

Represents a PondMate's emotional avatar.

- `FrogId`
- `RelationshipMemberId`
- `Name`
- `CurrentMood`
- `ActivityState`
- `LastUpdatedAt`
- `RelationshipMember`

Current Frog defaults:

- `Name`: `Little Frog`
- `CurrentMood`: `None`
- `ActivityState`: `Asleep`

`CurrentMood` and `ActivityState` are C# enums stored as readable text in
PostgreSQL.

### DailyCheckIn

Represents one PondMate's emotional check-in for a specific day inside a
Relationship/Pond.

- `DailyCheckInId`
- `RelationshipId`
- `UserAccountId`
- `CheckInDate`
- `PrimaryEmotion`
- `SecondaryEmotion`
- `TertiaryEmotion`
- `OptionalMessage`
- `CreatedAt`
- `UpdatedAt`
- `Relationship`
- `UserAccount`

Daily check-ins are unique by `RelationshipId`, `UserAccountId`, and
`CheckInDate`, so a PondMate can only create one check-in per Pond per day.

---

## Implemented Endpoints

### Health

```http
GET /api/health
```

### Auth

```http
POST /api/auth/register
POST /api/auth/login
POST /api/auth/logout
GET  /api/auth/me
```

### Relationship/Pond

```http
POST /api/relationship/create
GET  /api/relationship/mine
```

### Frog

```http
POST /api/frog/mine
GET  /api/frog/mine
PUT  /api/frog/mine
```

`POST /api/frog/mine` creates or returns the authenticated PondMate's default
Frog.

`GET /api/frog/mine` returns the authenticated PondMate's Frog.

`PUT /api/frog/mine` currently updates only the Frog name. Mood and activity
state are intentionally system-controlled so future check-in, Ribbit, and bug
gift behavior can update them safely.

Example update body:

```json
{
  "name": "Sprout"
}
```

### Daily Check-In

```http
POST /api/relationships/{relationshipId}/checkins
GET  /api/relationships/{relationshipId}/checkins/today/me
GET  /api/relationships/{relationshipId}/checkins/today
```

`POST /api/relationships/{relationshipId}/checkins` creates today's check-in
for the authenticated PondMate.

`GET /api/relationships/{relationshipId}/checkins/today/me` returns the
authenticated PondMate's check-in for today.

`GET /api/relationships/{relationshipId}/checkins/today` returns today's
check-in status for the Pond's members.

Example create body:

```json
{
  "primaryEmotion": "Happy",
  "secondaryEmotion": "Grateful",
  "tertiaryEmotion": "Hopeful",
  "optionalMessage": "Feeling pretty good today and glad we are building this together."
}
```

Creating a daily check-in also updates the authenticated PondMate's Frog:

- `CurrentMood` is mapped from the primary emotion.
- `ActivityState` becomes `Active`.

Protected endpoints use:

```http
Authorization: Bearer <token>
```

---

## Phase Summary

### Phase 1: Authentication Foundation - Complete

Implemented:

- User account creation
- Login
- Logout
- Password hashing
- Auth token generation
- Auth token validation
- Protected current-user endpoint
- Repository/service/controller structure
- Initial EF Core migration

### Phase 2: Relationship/Pond Foundation - Complete

Implemented:

- `Relationship` model
- `RelationshipMember` model
- Relationship creation
- Membership creation
- One active Pond guard for early v1 development
- Membership lookup
- Privacy helper logic to check whether a user belongs to a Relationship
- `POST /api/relationship/create`
- `GET /api/relationship/mine`
- EF migration for Relationship/Pond foundation

### Phase 3: Frog Foundation - Complete

Implemented:

- `Frog` model
- `FrogMood` enum
- `FrogActivityState` enum
- One Frog per `RelationshipMember`
- `DbSet<Frog>`
- Fluent API one-to-one relationship mapping
- Unique database constraint on `Frog.RelationshipMemberId`
- EF migration and database update
- Frog creation when a `RelationshipMember` is created
- Frog repository
- Frog service
- Frog response DTO
- Frog update request DTO
- `GET /api/frog/mine`
- `PUT /api/frog/mine`
- Postman tests for Frog retrieval and update flows

Frog privacy path:

```text
Bearer token -> UserAccount -> RelationshipMember -> Frog
```

The Frog endpoints do not accept arbitrary Frog IDs. They operate on the
authenticated user's own Frog through their RelationshipMember record.

### Phase 4: Daily Check-In - Complete

Implemented:

- `DailyCheckIn` model
- Emotion enum
- Primary, secondary, and tertiary emotions
- Optional message
- One check-in per user per Relationship per day
- Check-in creation endpoint
- Today's check-in status endpoint
- Update Frog mood/activity state after check-in
- Daily check-in repository
- Daily check-in service
- Daily check-in response DTOs
- PondMate check-in status DTO
- EF migration and unique constraint
- Postman tests for create, duplicate prevention, retrieval, Pond status, Frog update, and unauthorized access

Daily check-in privacy path:

```text
Bearer token -> UserAccount -> RelationshipMember -> Relationship -> DailyCheckIn
```

Check-in endpoints require the authenticated user to be a member of the
requested Relationship/Pond.

### Phase 5: Ribbits / Gentle Reminders - Next

Planned:

- `Ribbit` model
- Ribbit type enum
- Ribbit status enum
- Sender and receiver membership validation
- Create/send Ribbit endpoint
- Retrieve active Ribbits for a PondMate
- Acknowledge Ribbit endpoint
- Complete, decline, or cancel Ribbit behavior where applicable
- Frog notification indicator for unacknowledged Ribbits

---

## Postman

The Postman collection is included at:

```text
Postman/PondLite_Collection.postman_collection.json
```

Recommended high-level test order:

1. Register account
2. Log in
3. Get current user
4. Create Pond
5. Get my Pond
6. Get my Frog
7. Update my Frog
8. Create daily check-in
9. Confirm duplicate check-in is blocked
10. Get my today check-in
11. Get Pond check-in status
12. Confirm Frog state updated after check-in

The collection uses variables such as:

- `baseUrl`
- `testUsername`
- `testEmail`
- `testPassword`
- `token`
- `relationshipId`
- `frogId`
- `frogName`
- `dailyCheckInId`

---

## Local Development

Connection strings are managed through User Secrets and should not be committed
to GitHub.

The local database is expected to be named:

```text
pondlite_db
```

Common EF Core commands:

```powershell
dotnet ef migrations add <MigrationName>
dotnet ef database update
```

Common build command:

```powershell
dotnet build
```

---

## Security Notes

The current authentication system is intentionally simple for early development
and learning.

Implemented so far:

- Passwords are hashed before storage
- Raw passwords are not returned from the API
- Auth responses use DTOs
- Protected endpoints require bearer tokens
- Logout marks tokens inactive
- Relationship/Pond access is checked through RelationshipMember records

Future production work may include:

- ASP.NET Core Identity or another production-ready auth approach
- JWT bearer authentication or stronger session management
- Password reset
- Email verification
- Rate limiting
- Account deletion
- Data deletion
- Privacy policy and terms
- Stronger logging, monitoring, and backup practices

---

## Product Tone

PondLite should feel gentle, private, playful, and low-pressure.

Preferred language:

- Pond
- PondMate
- Frog
- Ribbit
- check-in
- care
- connection
- shared rhythm
- optional
- gentle reminder

Avoid punitive or guilt-based mechanics such as streak loss, shame language, or
anything that makes care feel like a chore.
