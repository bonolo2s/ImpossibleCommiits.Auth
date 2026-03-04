# ImpossibleCommits.Auth

> **Your vision, engineered to perfection.**

A modular, production-ready authentication library for ASP.NET Core — built by a developer who got tired of re-implementing auth from scratch on every project.

---

## The Story

Every .NET project needs auth. JWT, roles, password policies, rate limiting, audit logging — the same boilerplate, over and over again. Whether it's a fintech platform or a simple MVP, the foundation is always the same.

**ImpossibleCommits.Auth** was born out of that frustration. The goal is simple: drop it in, configure what you need, ignore what you don't. No bloat, no lock-in, no compromises.

Built flexible enough for production fintech systems. Simple enough for your next MVP.

---

## Features

### Core
- **JWT Authentication** — access tokens, refresh tokens, and automatic rotation
- **Token Binding** — ties tokens to a specific client/device, rejects stolen tokens used from a different origin *(toggleable)*
- **Password Policies** — configurable strength rules, expiry, and history enforcement
- **Password Recovery** — secure token-based reset flow
- **OAuth2 / OpenID Connect** — social and third-party login support *(toggleable)*

### Access Control
- **Role-Based Access Control (RBAC)** — flexible role definitions per project *(toggleable)*
- **Policy-Based Authorization** — define custom policies to match your business rules *(toggleable)*

### Advanced / Extras
- **Rate Limiting** — protect your endpoints from abuse *(toggleable)*
- **Audit Logging** — track login attempts, failed logins, token usage, and key auth events *(toggleable)*
  - Event hooks: registration, password change, token refresh, suspicious activity
- **Email Notifications** — bring your own email provider and templates *(toggleable)*
  - Registration confirmation
  - Password reset
  - Suspicious activity alerts
  - Custom sender address and content per notification type

---

## DB Agnostic

ImpossibleCommits.Auth works with any database. The library uses a generic `TKey` type parameter so your key type matches your DB:

| Database | Key Type |
|---|---|
| SQL Server / PostgreSQL | `Guid` |
| MongoDB | `string` |
| MySQL / SQLite | `int` or `Guid` |

Install the Core package plus the provider that matches your database:

```
ImpossibleCommits.Auth.Core          // always required
ImpossibleCommits.Auth.EFCore        // for SQL Server, PostgreSQL, MySQL, SQLite
ImpossibleCommits.Auth.MongoDB       // for MongoDB
```

---

## Extensible User Model

Your project has custom user fields. We know. Extend the base entity and pass your type in:

```csharp
public class AppUser : AuthUser<Guid>
{
    public string FullName { get; set; }
    public string ProfilePicture { get; set; }
}
```

Then register with your custom type:

```csharp
builder.Services.AddImpossibleAuth<AppUser, Guid>(options => { ... });
```

---

## Quick Start

### 1. Install packages

```bash
dotnet add package ImpossibleCommits.Auth.Core
dotnet add package ImpossibleCommits.Auth.EFCore
```

### 2. Register in Program.cs

```csharp
builder.Services.AddImpossibleAuth<AppUser, Guid>(options =>
{
    // JWT
    options.Jwt.AccessTokenExpiry = TimeSpan.FromMinutes(15);
    options.Jwt.EnableRotation = true;
    options.Jwt.EnableTokenBinding = true;

    // Password
    options.Password.MinLength = 8;
    options.Password.RequireSpecialChar = true;
    options.Password.ExpiryDays = 90;

    // Features
    options.EnableRateLimiting = true;
    options.EnableAuditLogging = true;
    options.EnableRBAC = true;
    options.EnablePolicyAuthorization = true;
    options.EnableOAuth = false;

    // Email
    options.Email.Enabled = true;
    options.Email.FromAddress = "noreply@yourapp.com";
    options.Email.FromName = "Your App";
})
.UseEFCore(connectionString)
.UseSqlServer();
```

### 3. Done.

Your auth layer is ready. Endpoints, token issuance, refresh, roles, policies — all wired up.

---

## Philosophy

- **Toggleable everything** — Don't load what you don't need
- **Configurable** — every default can be overridden
- **Modular** — Core is DB-agnostic, providers are separate packages
- **Fintech ready** — audit logging, token binding, rate limiting, suspicious activity detection
- **MVP friendly** — turn off everything you don't need and ship fast

---

## Roadmap

- [ ] v1 — Core auth, JWT, RBAC, EFCore + MongoDB providers
- [ ] v1.1 — OAuth2/OpenID support
- [ ] v1.2 — Email notifications
- [ ] v2 — Dapper provider, advanced audit log exporters

---

## Contributing

This library is open to contributions. If you find a bug or have a feature request, open an issue.

---

*ImpossibleCommits — Your vision, engineered to perfection.*
