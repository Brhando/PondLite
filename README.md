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
