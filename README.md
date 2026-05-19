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
- Phase 5: Ribbits / Gentle Reminders
- Phase 6: Discussion Prompts

Next planned phase:

- Phase 7: Bug-Themed Gift System

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

### Ribbit

Represents a gentle message, care offer, little nudge, or lightweight request
sent from one PondMate to another inside the same Relationship/Pond.

- `RibbitId`
- `RelationshipId`
- `SenderUserId`
- `ReceiverUserId`
- `Type`
- `Status`
- `Message`
- `AcknowledgementEmoji`
- `DeliveryTime`
- `CreatedAt`
- `AcknowledgedAt`
- `CompletedAt`
- `DeclinedAt`
- `CancelledAt`
- `Relationship`
- `SenderUser`
- `ReceiverUser`

Current Ribbit types:

- `Thought`
- `CareOffer`
- `GentleRequest`
- `LittleNudge`

Current Ribbit statuses:

- `Sent`
- `Acknowledged`
- `Completed`
- `Declined`
- `Cancelled`

`Completed`, `Declined`, and `Cancelled` are final states.

### DiscussionPrompt

Represents one shared discussion prompt for a Relationship/Pond on a specific
day.

- `DiscussionPromptId`
- `RelationshipId`
- `PromptText`
- `PromptDate`
- `CreatedAt`
- `Relationship`
- `Responses`

Discussion prompts are unique by `RelationshipId` and `PromptDate`, so a Pond
gets one shared prompt per day.

### DiscussionResponse

Represents one PondMate's response to a discussion prompt.

- `DiscussionResponseId`
- `DiscussionPromptId`
- `UserAccountId`
- `ResponseText`
- `CreatedAt`
- `UpdatedAt`
- `DiscussionPrompt`
- `UserAccount`

Discussion responses are unique by `DiscussionPromptId` and `UserAccountId`, so
each PondMate can have one response per prompt. Submitting again updates that
PondMate's existing response.

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
POST /api/relationship/join
GET  /api/relationship/mine
```

`POST /api/relationship/create` creates a new Pond and makes the authenticated
user the owner.

`POST /api/relationship/join` adds the authenticated user to an existing Pond if
they do not already belong to one and the Pond is not full.

Example join body:

```json
{
  "relationshipId": "00000000-0000-0000-0000-000000000000",
  "displayName": "PondMate"
}
```

`GET /api/relationship/mine` returns the authenticated user's active Pond.

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

### Ribbits

```http
POST  /api/ribbit
GET   /api/ribbit/active
GET   /api/ribbit/sent
PATCH /api/ribbit/{ribbitId}/acknowledge
PATCH /api/ribbit/{ribbitId}/complete
PATCH /api/ribbit/{ribbitId}/decline
PATCH /api/ribbit/{ribbitId}/cancel
```

`POST /api/ribbit` creates a Ribbit from the authenticated PondMate to another
PondMate in the same Relationship/Pond.

Example create body:

```json
{
  "receiverUserId": "00000000-0000-0000-0000-000000000000",
  "type": "GentleRequest",
  "message": "Could you send me a small check-in when you have a quiet moment?",
  "deliveryTime": null
}
```

`GET /api/ribbit/active` returns active Ribbits for the authenticated receiver.
Completed, declined, and cancelled Ribbits are excluded.

`GET /api/ribbit/sent` returns Ribbits sent by the authenticated sender.

`PATCH /api/ribbit/{ribbitId}/acknowledge` lets only the receiver acknowledge a
Ribbit.

Example acknowledge body:

```json
{
  "acknowledgementEmoji": "heart"
}
```

`PATCH /api/ribbit/{ribbitId}/complete` lets only the receiver complete a
Ribbit.

`PATCH /api/ribbit/{ribbitId}/decline` lets only the receiver decline a Ribbit.

`PATCH /api/ribbit/{ribbitId}/cancel` lets only the sender cancel a Ribbit.

Ribbit rules currently enforced by the backend:

- Auth is required for all Ribbit endpoints.
- Sender and receiver must both belong to the same Relationship/Pond.
- Sender cannot send a Ribbit to themselves.
- Only the receiver can acknowledge, complete, or decline a Ribbit.
- Only the sender can cancel a Ribbit.
- Completed, declined, and cancelled Ribbits are final states.
- Active Ribbits exclude completed, declined, and cancelled Ribbits.

### Discussion Prompts

```http
GET  /api/discussions/today
POST /api/discussions/today/response
```

`GET /api/discussions/today` returns today's shared prompt for the authenticated
PondMate's active Pond. If today's prompt does not exist yet, the backend creates
one for that Pond and date.

`POST /api/discussions/today/response` creates or updates the authenticated
PondMate's response to today's prompt.

Example response body:

```json
{
  "responseText": "I appreciated how you checked in with me today."
}
```

Discussion prompt rules currently enforced by the backend:

- Auth is required for Discussion endpoints.
- The prompt is scoped to the authenticated user's active Relationship/Pond.
- Empty response text is rejected.
- One response is stored per user per prompt.
- Submitting a second response updates the existing response instead of creating
  a duplicate.

Discussion prompt status responses include the prompt, response count, expected
response count, whether the current user has responded, and the current prompt's
responses.

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
- `POST /api/relationship/join`
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

### Phase 5: Ribbits / Gentle Reminders - Complete

Implemented:

- `Ribbit` model
- Ribbit type enum
- Ribbit status enum
- Sender and receiver membership validation
- Create/send Ribbit endpoint
- Retrieve active Ribbits for a PondMate
- Retrieve sent Ribbits for a PondMate
- Acknowledge Ribbit endpoint
- Complete, decline, or cancel Ribbit behavior where applicable
- Sender-only cancel rule
- Receiver-only acknowledge, complete, and decline rules
- Self-send prevention
- Receiver-outside-Pond prevention
- Final-state protection for completed, declined, and cancelled Ribbits
- Active Ribbits exclude completed, declined, and cancelled Ribbits
- EF migration and indexes for Ribbit lookup
- Postman tests for Ribbit happy paths, authorization failures, membership rules, wrong-user state changes, final-state protection, active Ribbits, and sent Ribbits

Ribbit privacy path:

```text
Bearer token -> UserAccount -> RelationshipMember -> Relationship -> Ribbit
```

Ribbits are only valid when sender and receiver are both PondMates in the same
Relationship/Pond.

### Phase 6: Discussion Prompts - Complete

Implemented:

- `DiscussionPrompt` model
- `DiscussionResponse` model
- Daily prompt retrieval
- Submit discussion response endpoint
- Retrieve responses for the current prompt
- Track prompt response status for PondMates
- Unique response rule per prompt and user
- Update existing response when the same PondMate submits again
- Auth checks for Discussion endpoints
- Empty response validation
- EF migration, unique constraints, and delete behavior
- Postman tests for retrieval, response creation, response update, missing token,
  invalid token, and empty response text

Discussion prompt privacy path:

```text
Bearer token -> UserAccount -> RelationshipMember -> Relationship -> DiscussionPrompt
```

Discussion prompts are scoped through the authenticated user's active Pond
rather than accepting arbitrary Relationship IDs from the client.

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
13. Join a second PondMate to the Pond
14. Create Ribbits between PondMates
15. Acknowledge, complete, decline, and cancel Ribbits
16. Confirm Ribbit access and final-state rules
17. Get today's Discussion Prompt
18. Submit and update today's Discussion response
19. Confirm Discussion authorization and validation rules

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
- `ribbitSenderToken`
- `ribbitReceiverToken`
- `ribbitOutsiderToken`
- `ribbitSenderUserAccountId`
- `ribbitReceiverUserAccountId`
- `ribbitOutsiderUserAccountId`
- `ribbitId`
- `discussionPromptId`
- `discussionResponseId`
- `discussionResponseText`
- `updatedDiscussionResponseText`

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
