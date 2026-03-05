# ImpossibleCommits.Auth

> **Your vision, engineered to perfection.**

A modular, production-ready authentication library for ASP.NET Core — built by a developer who got tired of re-implementing auth from scratch on every project.

---

## The Story

Every .NET project needs auth. JWT, roles, password policies, rate limiting, audit logging — the same boilerplate, over and over again. Whether it's a fintech platform or a simple MVP, the foundation is always the same.

**ImpossibleCommits.Auth** was born out of that frustration. The# ImpossibleCommits.Auth

> **Your vision, engineered to perfection.**

A lightweight, stateless authentication toolbox for ASP.NET Core — built by a developer who got tired of re-implementing auth from scratch on every project.

---

## The Story

Every .NET project needs auth. Password hashing, JWT generation, rate limiting, audit logging, password resets — the same boilerplate, over and over again. Whether it's a fintech platform or a weekend MVP, the foundation is always the same.

**ImpossibleCommits.Auth** was born out of that frustration. The goal is simple: a library of battle-tested auth helper methods you call when you need them. No DB lock-in, no forced architecture, no bloat. Drop it in, configure what you need, ignore what you don't.

---

## How It Works

This library is a **stateless toolbox** — it does not touch your database, own your models, or dictate your architecture.

You stay in control:
- You collect user input in your own controllers
- You call the library method to do the heavy lifting
- You get back a result and decide what to do with it
- You store or use the data however you want

No migrations. No base classes to extend. No providers to install. Just methods.

---

## Features

### Password
- **Hash a password** — secure hashing out the box
- **Verify a password** — compare plain text against a stored hash
- **Password strength enforcement** — configurable rules (length, special chars, uppercase, etc.)
- **Password expiry** — enforce rotation policies
- **Password reset tokens** — generate and validate secure reset tokens

### JWT
- **Generate access tokens** — configurable expiry, claims, issuer, audience
- **Generate refresh tokens** — with rotation support
- **Validate tokens** — returns claims or rejection reason
- **Token binding** — tie tokens to a specific client/device *(toggleable)*

### Email Notifications *(toggleable)*
- Registration confirmation
- Password reset
- Suspicious activity alert
- Pass your own SMTP config — library handles the sending

### Rate Limiting *(toggleable)*
- Protect any endpoint or operation
- Configurable max requests and window

### Audit Logging *(toggleable)*
- Returns structured audit data on key events
- Login attempts, failures, token usage, password changes
- You persist it wherever you want — your DB, a file, a cloud logger

### Access Control *(toggleable)*
- Role-based access control helpers
- Policy-based authorization helpers

---

## Quick Start

### 1. Install

```bash
dotnet add package ImpossibleCommits.Auth
```

### 2. Register in Program.cs

```csharp
builder.Services.AddImpossibleAuth(options =>
{
    // JWT
    options.Jwt.Secret = "your-secret-key";
    options.Jwt.AccessTokenExpiry = TimeSpan.FromMinutes(15);
    options.Jwt.RefreshTokenExpiry = TimeSpan.FromDays(7);
    options.Jwt.EnableRotation = true;
    options.Jwt.EnableTokenBinding = false;

    // Password
    options.Password.MinLength = 8;
    options.Password.RequireSpecialChar = true;
    options.Password.RequireUppercase = true;
    options.Password.ExpiryDays = 90; // null = never expires

    // Email
    options.Email.Enabled = true;
    options.Email.FromAddress = "noreply@yourapp.com";
    options.Email.FromName = "Your App";
    options.Email.SmtpHost = "smtp.yourprovider.com";
    options.Email.SmtpPort = 587;

    // Features
    options.EnableRateLimiting = true;
    options.EnableAuditLogging = true;
    options.EnableRBAC = false;
});
```

### 3. Use in your controllers

```csharp
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;

    public AuthController(IAuthService auth) => _auth = auth;

    [HttpPost("register")]
    public IActionResult Register(RegisterDto dto)
    {
        var hash = _auth.HashPassword(dto.Password);
        // store hash in your own DB however you want
        return Ok(new { PasswordHash = hash });
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        // fetch user from your own DB
        var user = _yourUserRepo.GetByEmail(dto.Email);

        var valid = _auth.VerifyPassword(dto.Password, user.PasswordHash);
        if (!valid) return Unauthorized();

        var token = _auth.GenerateToken(user.Id.ToString(), claims: new[]
        {
            new Claim("email", user.Email)
        });

        return Ok(token);
    }
}
```

---

## Philosophy

- **Stateless by design** — no DB, no migrations, no lock-in
- **Toggleable everything** — don't load what you don't need
- **Configurable** — every default can be overridden
- **You own your data** — library returns results, you decide what to do with them
- **Built for any scale** — fintech, healthcare, e-commerce, or a weekend MVP

---

## Roadmap

- [ ] v1 — Password hashing, JWT generation & validation, password reset tokens
- [ ] v1.1 — Email notifications (registration, reset, suspicious activity)
- [ ] v1.2 — Rate limiting, audit logging
- [ ] v2 — OAuth2/OpenID Connect, RBAC helpers, policy authorization helpers

---

## Contributing

Open to contributions. If you find a bug or have a feature request, open an issue.

---

*ImpossibleCommits — Your vision, engineered to perfection.* goal is simple: drop it in, configure what you need, ignore what you don't. No bloat, no lock-in, no compromises.

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
- **Built for any environment that demands reliability — fintech, healthcare or an e-commerce** — audit logging, token binding, rate limiting, suspicious activity detection
- **Weekend MVP friendly** — turn off everything you don't need and ship fast

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
