# ImpossibleCommits.Auth

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
- **Validate tokens** — returns claims or null
- **Token binding** — tie tokens to a specific client/device *(configurable)*
- **Revoke tokens** — invalidate tokens on logout or suspicious activity

### Rate Limiting
- **User-aware limiting** — limit by userId, not IP
- **Per action** — login, password reset, token refresh each have independent counters
- **Configurable window and max attempts** per action type
- You pass in the attempt count from your own storage — library evaluates, stays stateless

### Audit Logging
- Returns structured `AuditEvent` data on key auth events
- Login attempts, failed logins, token usage, token refresh, token revocation
- Password changes, password reset requests, suspicious activity, logout
- You persist it wherever you want — your DB, a file, a cloud logger

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
    options.Jwt.Issuer = "your-app";
    options.Jwt.Audience = "your-users";
    options.Jwt.AccessTokenExpiry = TimeSpan.FromMinutes(15);
    options.Jwt.RefreshTokenExpiry = TimeSpan.FromDays(7);
    options.Jwt.EnableRotation = true;
    options.Jwt.EnableTokenBinding = false;

    // Password
    options.Password.MinLength = 8;
    options.Password.RequireUppercase = true;
    options.Password.RequireSpecialChar = true;
    options.Password.ExpiryDays = 90; // null = never expires

    // Rate Limiting
    options.RateLimit.MaxLoginAttempts = 5;
    options.RateLimit.MaxPasswordResetAttempts = 3;
    options.RateLimit.Window = TimeSpan.FromMinutes(10);

    // Audit
    options.Audit.LogSuccessfulLogins = true;
    options.Audit.LogFailedLogins = true;
    options.Audit.LogSuspiciousActivity = true;
});
```

### 3. Use in your controllers

```csharp
public class AuthController : ControllerBase
{
    private readonly IAuthService _auth;
    private readonly IAuditService _audit;

    public AuthController(IAuthService auth, IAuditService audit)
    {
        _auth = auth;
        _audit = audit;
    }

    [HttpPost("register")]
    public IActionResult Register(RegisterDto dto)
    {
        var hash = _auth.HashPassword(dto.Password);
        // store hash in your own DB
        return Ok(new { PasswordHash = hash });
    }

    [HttpPost("login")]
    public IActionResult Login(LoginDto dto)
    {
        // fetch user from your own DB
        var user = _yourUserRepo.GetByEmail(dto.Email);

        var valid = _auth.VerifyPassword(dto.Password, user.PasswordHash);

        var auditEvent = _audit.LogLoginAttempt(user.Id.ToString(), HttpContext.Connection.RemoteIpAddress?.ToString(), valid);
        // persist auditEvent in your own DB

        if (!valid) return Unauthorized();

        var token = _auth.GenerateToken(user.Id.ToString(), new[]
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
- **Configurable** — every default can be overridden
- **You own your data** — library returns results, you decide what to do with them
- **Built for any scale** — fintech, healthcare, e-commerce, or a weekend MVP

---

## Roadmap

- [ ] v1 — Password hashing, JWT generation & validation, password reset tokens, rate limiting helpers, audit logging helpers
- [ ] v1.1 — OAuth2/OpenID Connect support
- [ ] v2 — Advanced token binding, anomaly detection helpers

---

## Contributing

Open to contributions. If you find a bug or have a feature request, open an issue.

---

*ImpossibleCommits — Your vision, engineered to perfection.*
