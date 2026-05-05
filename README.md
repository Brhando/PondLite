# PondLite

PondLite is a low-pressure relationship connection app designed to help partners stay emotionally connected throughout the day without turning care into a chore. The goal is to create a small, meaningful app where users can complete daily emotional check-ins, view a shared “pond,” send gentle reminders/ribbits, respond to discussion prompts, and eventually receive smart connection suggestions from an AI assistant.

This project is also being used as a personal learning project to practice full-stack application development with a Microsoft/.NET-focused backend stack.

---

## Project Goals

The main goals of PondLite are to:

- Help partners stay connected in a gentle, non-punitive way
- Support daily emotional check-ins using selected emotions and optional messages
- Represent the relationship through a shared pond and frog state
- Allow small reminders or “ribbits” between partners
- Provide daily discussion prompts
- Explore an AI-powered smart connection assistant
- Practice backend architecture using ASP.NET Core, EF Core, PostgreSQL, repositories, DTOs, and service layers

---

## Current Status

Phase 1, **Authentication**, is complete.

The project currently includes:

- ASP.NET Core Web API backend
- PostgreSQL database integration
- Entity Framework Core migrations
- Repository layer
- User account creation
- Login
- Logout
- Bearer token generation
- Server-side token validation
- Password hashing
- Protected current-user lookup endpoint
- Postman-tested authentication flow

Current implemented endpoints include:

```http
GET  /api/health
POST /api/auth/register
POST /api/auth/login
POST /api/auth/logout
GET  /api/auth/me
```

---

## Tech Stack

### Backend

- C#
- ASP.NET Core Web API
- Entity Framework Core
- PostgreSQL
- Npgsql PostgreSQL EF Core provider
- ASP.NET Core password hashing utilities
- Repository/service/controller architecture

### Development Tools

- Visual Studio
- pgAdmin 4
- Postman
- Git/GitHub

### Planned Frontend

The frontend has not been implemented yet. Possible frontend paths include:

- React
- Blazor
- Mobile-friendly web app / PWA
- Future native or hybrid mobile deployment option

---

## Architecture Overview

The current backend follows a layered structure:

```text
Controllers
    ↓
Services / Interfaces
    ↓
Repositories / Interfaces
    ↓
Entity Framework Core DbContext
    ↓
PostgreSQL
```

Current authentication flow:

```text
AuthController
    ↓
IAuthService
    ↓
AuthService
    ↓
IUserAccountRepository / IAuthTokenRepository
    ↓
EfUserAccountRepository / EfAuthTokenRepository
    ↓
PondLiteDbContext
    ↓
PostgreSQL
```

This structure keeps HTTP handling, business logic, and data access separated.

---

## Implemented Models

### UserAccount

Represents a registered PondLite user.

Current fields include:

- AccountId
- Username
- Email
- PasswordHash
- CreatedAt

### AuthToken

Represents an active or expired login/session token.

Current fields include:

- TokenId
- UserAccountId
- TokenValue
- CreatedAt
- ExpiresAt
- IsActive

---

## Development Phases

### Phase 1: Auth — Complete

Implemented:

- UserAccount
- AuthToken
- Create account
- Login
- Logout
- Bearer token creation
- Token validation
- Password hashing
- PostgreSQL persistence

### Phase 2: Pond Foundation — Next

Planned models/features:

- Pond
- PondMember
- Frog

This phase will establish the shared relationship space that both users belong to.

### Phase 3: Daily Check-In

Planned models/features:

- DailyCheckIn
- Emotion enum
- Frog mood update logic

Users will be able to complete a daily check-in by selecting emotions and optionally writing a short message.

### Phase 4: Reminder/Ribbit

Planned models/features:

- ReminderRibbit
- ReminderStatus enum
- ReminderType enum

Users will be able to send small reminders or “ribbits” to their PondMate.

### Phase 5: Discussion Prompt

Planned models/features:

- DiscussionPrompt
- DiscussionResponse

Users will be able to answer daily prompts and view each other’s responses.

### Phase 6: Smart Connection Assistant

Planned models/features:

- SmartConnectionSuggestion
- AI request/response DTOs

This feature will use recent relationship context to suggest care actions, message ideas, or discussion prompts.

---

## Local Development Notes

The app currently uses a local PostgreSQL database.

Database connection strings are managed through **User Secrets** and should not be committed to GitHub.

The local database is currently expected to be named:

```text
pondlite_db
```

The database schema is managed through Entity Framework Core migrations.

Common EF Core commands:

```powershell
Add-Migration InitialCreate
Update-Database
```

---

## Current Testing Flow

The current auth flow has been tested manually in Postman.

Recommended test order:

1. Register a new user
2. Log in as that user
3. Copy the returned bearer token
4. Call `/api/auth/me` with the token
5. Log out with the token
6. Call `/api/auth/me` again and confirm the token is invalid

Bearer token format:

```http
Authorization: Bearer <token>
```

---

## Security Notes

The current authentication system is intentionally simple and educational.

Implemented so far:

- Passwords are hashed before being stored
- Raw passwords are not returned from the API
- Auth responses use DTOs
- Protected endpoints require a bearer token
- Logout marks tokens inactive

Future security improvements may include:

- ASP.NET Core Identity
- Built-in ASP.NET Core authentication/authorization middleware
- JWT bearer authentication
- Refresh tokens
- Stronger validation
- Authorization policies
- More production-ready token/session management

---

## Long-Term Vision

The long-term vision for PondLite is to become a small, personal app that can help two partners stay connected through gentle emotional awareness, small acts of care, and shared reflection.

The app is designed to avoid guilt-based or punitive mechanics. Instead, it focuses on small moments of connection, emotional visibility, and playful relationship growth.
