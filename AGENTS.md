# AGENTS.md — PondLite Project Instructions

## Project Context

PondLite is a private relationship app for couples. The backend/domain term is `Relationship`; the user-facing term is `Pond`. A `RelationshipMember` is a user connected to a Relationship/Pond.

Design docs are stored in:

- `DesignDocs_and_Schedule/overview_and_features.pdf`
- `DesignDocs_and_Schedule/user_stories.pdf`
- `DesignDocs_and_Schedule/use_cases.pdf`
- `DesignDocs_and_Schedule/domain_model.pdf`
- `DesignDocs_and_Schedule/privacy_and_access_control_rules.pdf`
- `DesignDocs_and_Schedule/schedule.pdf`

Before adding or changing major features, review the relevant design doc first.

## Current Architecture

Backend stack:

- ASP.NET Core
- C#
- Entity Framework Core
- PostgreSQL
- Manual bearer-token authentication for early development

Preferred architecture flow:

Controller → Service → Repository → DbContext → PostgreSQL

Follow the existing structure:

- Controllers receive HTTP requests and stay thin.
- Services contain business rules.
- Repositories handle database access.
- DTOs shape request and response data.
- EF entities should not be returned directly from controllers.

## Privacy Rules

Privacy is a core requirement.

Authentication answers:

> Who is this user?

Relationship membership answers:

> What Pond can this user access?

For any Relationship/Pond-owned data, verify that the authenticated user is a `RelationshipMember` of that Relationship before returning, creating, updating, or deleting private data.

Do not rely on frontend-only privacy.

## Product Tone

PondLite should feel gentle, private, playful, and low-pressure.

Use language like:

- Pond
- PondMate
- Ribbit
- check-in
- care
- connection
- shared rhythm
- optional
- gentle reminder

Avoid language like:

- failed
- punishment
- overdue
- missed streak
- bad partner
- required task

Do not add punitive streaks, guilt-based mechanics, or shame language.

## Learning Workflow

The user is learning and wants to understand the code.

When making changes:

1. Explain the goal of the change first.
2. Show the smallest reasonable code change.
3. Explain why the change is needed.
4. Point out the primary key, foreign key, navigation property, and Fluent API reasoning when EF Core relationships are involved.
5. Pause after meaningful steps instead of dumping an entire feature all at once.
6. Prefer incremental, testable changes.
7. Include Postman testing guidance after adding endpoints.

Do not rewrite large sections of working code unless explicitly asked.

## EF Core Guidance

Use Fluent API when relationships, delete behavior, unique constraints, or important business rules should be explicit.

For new models:

1. Decide if the model needs its own primary key.
2. Decide what entity it belongs to.
3. Add foreign keys for parent relationships.
4. Add navigation properties when related objects need to be loaded.
5. Register the `DbSet` in `PondLiteDbContext`.
6. Add Fluent API rules in `OnModelCreating` when needed.
7. Add a migration and update the database.
