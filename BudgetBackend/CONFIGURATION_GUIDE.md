# Environment Configuration & Secrets Management

## Overview
This document explains how to safely manage configuration and secrets across development, staging, and production environments.

---

## File Structure

```
BudgetBackend/
??? appsettings.json              # Base configuration (development defaults)
??? appsettings.Development.json  # Local development (if you create it)
??? appsettings.Production.json   # Production settings (templates)
??? Program.cs                    # Application startup
??? .gitignore                   # Ensure secrets aren't committed
```

---

## appsettings.json Structure

This is the **base configuration** that applies to all environments:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=Budget;Username=postgres;Password=12345678"
  },
  "JwtSettings": {
    "SecretKey": "your-super-secret-key-that-is-at-least-32-characters-long-for-hs256",
    "Issuer": "BudgetBackend",
    "Audience": "BudgetApp",
    "TokenExpirationMinutes": 480
  },
  "Cors": {
    "AllowedOrigins": ["http://localhost:4200"]
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning",
      "Microsoft.EntityFrameworkCore": "Information"
    }
  }
}
```

---

## appsettings.Production.json

This **overrides** values from `appsettings.json` when `ASPNETCORE_ENVIRONMENT=Production`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "WILL_BE_SET_BY_ENV_VARS"
  },
  "JwtSettings": {
    "SecretKey": "WILL_BE_SET_BY_ENV_VARS",
    "TokenExpirationMinutes": 480
  },
  "Cors": {
    "AllowedOrigins": [
      "https://your-angular-app.com",
      "https://www.your-angular-app.com"
    ]
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

---

## How Configuration Works in Program.cs

```csharp
var environment = builder.Environment.EnvironmentName;  // Gets ASPNETCORE_ENVIRONMENT

// Loads appsettings.json then appsettings.{environment}.json
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true)
    .AddEnvironmentVariables();  // Environment variables override everything
```

### Loading Order (Priority: Top = Highest)
1. **Environment Variables** ? Use these for secrets in production
2. **appsettings.{Environment}.json** ? e.g., `appsettings.Production.json`
3. **appsettings.json** ? Base configuration

---

## Setting Environment Variables

### Platform: Railway

In Railway dashboard, Variables section:
```
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=postgresql://user:pass@host:5432/db
JwtSettings__SecretKey=your-generated-secret-key
JwtSettings__Issuer=BudgetBackend
JwtSettings__Audience=BudgetApp
Cors__AllowedOrigins__0=https://your-frontend.com
```

### Platform: Render

In Web Service settings ? Environment:
```
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=postgresql://...
JwtSettings__SecretKey=...
```

### Platform: Azure

In App Service ? Configuration ? Application settings:
| Name | Value |
|------|-------|
| ASPNETCORE_ENVIRONMENT | Production |
| ConnectionStrings__DefaultConnection | postgresql://... |
| JwtSettings__SecretKey | ... |

### Local Development (Command Line)

```bash
# Windows (PowerShell)
$env:ASPNETCORE_ENVIRONMENT="Development"
dotnet run

# Windows (CMD)
set ASPNETCORE_ENVIRONMENT=Development
dotnet run

# macOS/Linux (Bash)
export ASPNETCORE_ENVIRONMENT=Development
dotnet run
```

### Local Development (Visual Studio)

1. Debug ? Properties
2. Set `ASPNETCORE_ENVIRONMENT` under environment variables

### Local Development (.NET User Secrets)

For development, use User Secrets instead of env vars:

```bash
# Enable user secrets for project
dotnet user-secrets init

# Set secrets (stored securely, not in code)
dotnet user-secrets set "JwtSettings:SecretKey" "your-secret-key"
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "your-connection-string"

# View all secrets
dotnet user-secrets list

# Remove secret
dotnet user-secrets remove "JwtSettings:SecretKey"
```

User secrets are stored in:
- **Windows**: `%APPDATA%\Microsoft\UserSecrets\<project-id>\secrets.json`
- **macOS/Linux**: `~/.microsoft/usersecrets/<project-id>/secrets.json`

Never commit these files - they're already in `.gitignore`!

---

## Configuration by Scenario

### Scenario 1: Local Development
```
appsettings.json (use localhost, local database)
?
No environment variable set (defaults to Development)
?
VS debugger runs with local values
```

**Setup:**
```bash
# Terminal - start PostgreSQL locally
docker run -e POSTGRES_PASSWORD=password -p 5432:5432 postgres

# Run application
dotnet run
```

### Scenario 2: Docker Local Testing
```
appsettings.json (development)
?
docker-compose.yml overrides with Docker values
?
Tests before cloud deployment
```

**Setup:**
```bash
docker compose up
# Backend: http://localhost:5000
# Database: localhost:5432
```

### Scenario 3: Production Deployment

```
appsettings.json (base defaults)
    ?
appsettings.Production.json (production overrides)
    ?
Platform environment variables (highest priority)
    ?
= Final configuration used at runtime
```

**Setup:**
1. Platform creates PostgreSQL instance
2. Set `ASPNETCORE_ENVIRONMENT=Production` in platform
3. Set all secrets in platform's env var management
4. Deploy code - configuration automatically loaded

---

## Environment Variable Naming

When using environment variables, replace `:` with `__` (double underscore):

**Config File:**
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "..."
  },
  "JwtSettings": {
    "SecretKey": "..."
  },
  "Cors": {
    "AllowedOrigins": ["...", "..."]
  }
}
```

**Environment Variables:**
```
ConnectionStrings__DefaultConnection=value
JwtSettings__SecretKey=value
JwtSettings__Issuer=value
Cors__AllowedOrigins__0=first-origin
Cors__AllowedOrigins__1=second-origin
```

---

## Accessing Configuration in Code

### In Program.cs or Services

```csharp
// Access individual settings
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Access sections
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"];

// Access with default fallback
var audience = builder.Configuration["JwtSettings:Audience"] ?? "DefaultAudience";

// Access arrays
var origins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();
```

---

## Secrets Generation

### Strong JWT Secret (32+ characters)

**Option 1: PowerShell**
```powershell
[Convert]::ToBase64String((1..64 | ForEach-Object { [byte](Get-Random -Max 256) }))
```

**Option 2: Linux/macOS**
```bash
openssl rand -base64 32
```

**Option 3: Online Generator**
Visit: https://generate-random.org (set to 64 characters, copy)

### Strong Database Password

**Option 1: PowerShell**
```powershell
[Convert]::ToBase64String((1..32 | ForEach-Object { [byte](Get-Random -Max 256) })) | Select -First 32
```

**Option 2: Online Generator**
Visit: https://www.random.org/strings/ 

---

## Security Best Practices

### ? DO:
- [ ] Store secrets in environment variables on production platforms
- [ ] Use user secrets for local development (`dotnet user-secrets`)
- [ ] Rotate JWT secrets regularly
- [ ] Use strong passwords (32+ characters)
- [ ] Enable SSL/HTTPS (all platforms do this)
- [ ] Use `appsettings.Production.json` for production-specific values
- [ ] Different secret for each environment
- [ ] Different database credentials for each environment
- [ ] Regenerate secrets if ever leaked

### ? DON'T:
- [ ] Hardcode secrets in appsettings.json
- [ ] Commit secrets to GitHub
- [ ] Share secrets via chat/email/messages
- [ ] Use simple passwords like "password123"
- [ ] Have same secret across environments
- [ ] Check in `.user` files (IDE secrets)
- [ ] Log sensitive data
- [ ] Share database credentials

---

## .gitignore Configuration

Ensure this is in your `.gitignore`:

```gitignore
# Build results
bin/
obj/

# IDE
.vs/
.vscode/
*.user
*.suo
*.swp

# Secrets (User Secrets)
usersecrets/
.vs/

# Environment files (if you create specific ones)
appsettings.local.json

# Any local overrides
local-appsettings.json
```

---

## Troubleshooting Configuration Issues

### Problem: "Invalid connection string"
**Solution**: Check format matches your platform
- PostgreSQL: `postgresql://user:pass@host:5432/db`
- Copy full string from platform (Railway/Render/Azure)

### Problem: "JwtSettings__SecretKey not found"
**Solution**: Ensure environment variable name uses `__` (not `:`)
- Wrong: `JwtSettings:SecretKey`
- Right: `JwtSettings__SecretKey`

### Problem: "CORS blocked"
**Solution**: Verify exact domain in `Cors__AllowedOrigins__0`
- Must match frontend exactly: `https://example.com` (no trailing slash)
- Change `localhost` to real domain in production

### Problem: "Configuration not loading"
**Solution**: Check `ASPNETCORE_ENVIRONMENT` is set correctly
```bash
# Check what environment is active
echo $env:ASPNETCORE_ENVIRONMENT  # PowerShell
echo $ASPNETCORE_ENVIRONMENT      # Bash
```

---

## Checking What Configuration is Active

Add this to Program.cs for debugging:

```csharp
Console.WriteLine($"Environment: {builder.Environment.EnvironmentName}");
Console.WriteLine($"Connection String: {builder.Configuration.GetConnectionString("DefaultConnection")}");
Console.WriteLine($"JWT Issuer: {builder.Configuration["JwtSettings:Issuer"]}");
Console.WriteLine($"CORS Origins: {string.Join(", ", builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())}");
```

This prints actual configuration at startup.

---

## Configuration Files Checklist

Before deploying:

- [ ] `appsettings.json` - Has development defaults (safe for public)
- [ ] `appsettings.Production.json` - Has production overrides (uses env vars)
- [ ] `.gitignore` - Includes all secret files
- [ ] Environment variables set in platform dashboard
- [ ] Connection string format correct for target database
- [ ] JWT secret is strong (32+ characters)
- [ ] CORS origins correct (frontend domain)
- [ ] No secrets in code
- [ ] No hardcoded connection strings

---

## Migration from Development to Production

1. **Verify locally** - `dotnet run` with user secrets works
2. **Test with Docker** - `docker compose up` works
3. **Create account** on production platform (Railway/Render/Azure)
4. **Generate new secrets** for production
5. **Set environment variables** in platform dashboard
6. **Update CORS origins** for production frontend domain
7. **Deploy** - Push to GitHub or use platform CLI
8. **Monitor logs** - Check for configuration errors
9. **Test endpoints** - Verify API works with new config
10. **Enable backups** - Database backups enabled

---

## Quick Reference: Environment Variables

```bash
# Required
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=postgresql://...
JwtSettings__SecretKey=your-strong-secret

# Optional
JwtSettings__Issuer=BudgetBackend
JwtSettings__Audience=BudgetApp
Cors__AllowedOrigins__0=https://frontend-url.com
Logging__LogLevel__Default=Information
```

---

For more information:
- Microsoft Docs: https://docs.microsoft.com/en-us/dotnet/core/runtime-config/
- User Secrets: https://learn.microsoft.com/en-us/aspnet/core/security/app-secrets
- Configuration Providers: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration
