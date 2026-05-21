# Production Deployment Quick Reference

## Quick Platform Comparison

| Aspect | Railway | Render | Azure |
|--------|---------|--------|-------|
| **Setup Time** | 5 min | 10 min | 15 min |
| **Free Tier** | Yes ($5 credits) | Yes | Yes (12 months) |
| **Monthly Cost** | $5-20 | $7-30 | $10-50 |
| **Ease of Use** | ????? | ???? | ??? |
| **Scalability** | Good | Good | Excellent |
| **Learning Curve** | Very Low | Low | Medium |
| **PostgreSQL** | Included | Included | Separate service |
| **Monitoring** | Basic | Basic | Advanced |
| **Best For** | MVPs, Prototypes | Small-Medium Apps | Enterprise |

---

## Environment Variables Template

Create these variables in your chosen platform:

```bash
# Required
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=postgresql://user:password@host:5432/database
JwtSettings__SecretKey=generate-strong-key-min-32-chars
JwtSettings__Issuer=BudgetBackend
JwtSettings__Audience=BudgetApp
Cors__AllowedOrigins__0=https://your-angular-domain.com

# Optional
Logging__LogLevel__Default=Information
Logging__LogLevel__Microsoft=Warning
```

---

## PostgreSQL Connection String Format

```
postgresql://username:password@hostname:5432/databasename
```

### Example Variations:
- **Local**: `postgresql://postgres:password@localhost:5432/budget`
- **Railway**: `postgresql://root:password@railway-container:5432/railway`
- **Render**: `postgresql://user:pass@dpg-xxxxx.postgres.render.com:5432/db`
- **Azure**: `Host=server.postgres.database.azure.com;Port=5432;Database=budget;Username=user@server;Password=pass;SSL Mode=Require;`

---

## Docker Deployment (Any Platform Supporting Containers)

### Build and Test Locally:
```bash
docker build -f BudgetBackend/Dockerfile -t budgetbackend:latest .
docker compose up
```

### Push to Container Registry:
```bash
# Docker Hub
docker tag budgetbackend:latest yourusername/budgetbackend:latest
docker push yourusername/budgetbackend:latest

# Azure Container Registry
az acr build --registry myregistry --image budgetbackend:latest .
```

---

## Health Check Endpoint

All platforms should monitor this endpoint:

```
GET /health
```

Response (if healthy):
```
HTTP 200 OK
```

---

## JWT Secret Key Generation

Generate a secure key for production:

**Option 1: PowerShell**
```powershell
[Convert]::ToBase64String((1..64 | ForEach-Object { [byte](Get-Random -Max 256) }) ) | out-string
```

**Option 2: OpenSSL**
```bash
openssl rand -base64 64
```

**Option 3: Online Generator**
Visit: https://generate-random.org (and select at least 32 characters)

---

## Pre-Deployment Checklist

### Code:
- [ ] All services registered in Program.cs
- [ ] appsettings.Production.json updated with correct domains
- [ ] CORS origins point to actual frontend URL (not localhost)
- [ ] JWT secret is strong (minimum 32 characters)
- [ ] No hardcoded secrets in code

### Database:
- [ ] PostgreSQL running and accessible
- [ ] Connection string correct
- [ ] Database user has proper permissions
- [ ] Migrations tested locally

### Frontend Integration:
- [ ] Angular environment.prod.ts points to production API URL
- [ ] API URL includes `/api` prefix if configured
- [ ] CORS headers will be accepted by browser

### Testing:
- [ ] Run migrations locally to verify they work
- [ ] Test API with Postman using production-like setup
- [ ] Test JWT token flow (login ? use token ? access protected endpoint)
- [ ] Test CORS by calling from frontend domain

---

## Platform-Specific Commands

### Railway
```bash
# Login
railway login

# Create service
railway service

# Set environment variables
railway variable set ASPNETCORE_ENVIRONMENT Production

# Deploy
git push
```

### Render
- Use Render dashboard - minimal CLI needed
- Set environment vars in web service settings
- Deploys automatically on git push

### Azure
```bash
# Login
az login

# Deploy app
az webapp up --name budgetbackend-prod --resource-group MyResourceGroup

# Set secrets
az webapp config appsettings set --resource-group MyResourceGroup --name budgetbackend-prod --settings "ConnectionStrings__DefaultConnection=YOUR_CONNECTION_STRING"
```

### Docker
```bash
# Build
docker build -f BudgetBackend/Dockerfile -t budgetbackend:latest .

# Run
docker run -e ConnectionStrings__DefaultConnection="your_string" -p 5000:5000 budgetbackend:latest

# Compose (local testing)
docker compose up
```

---

## Typical Deployment Costs (Monthly)

### Minimal Setup (MVP):
- Railway: **Free tier** or **$5-10/month**
- Render: **Free tier** or **$7-15/month**
- Azure: **Free tier (12 months)** then **$15-30/month**

### Standard Setup (Production):
- Railway: **$15-30/month**
- Render: **$20-40/month**
- Azure: **$40-80/month** (includes advanced monitoring)

### Scaling Setup (Heavy Load):
- Railway: **$50+/month**
- Render: **$100+/month**
- Azure: **$200+/month** (with full auto-scaling)

---

## Troubleshooting

### Database Connection Issues
```bash
# Test connection string format
# PostgreSQL: postgresql://[user]:[password]@[host]:[port]/[database]

# Check platform allows outbound connections to database
# Usually requires firewall rules or connection pooling
```

### CORS Errors in Browser Console
```
Access to XMLHttpRequest blocked by CORS

Solution: 
1. Ensure frontend URL in Cors__AllowedOrigins matches EXACTLY
2. No trailing slashes: https://example.com (not https://example.com/)
3. No localhost in production: use actual domain
```

### JWT Token Validation Fails
```
Invalid token / Token validation failed

Solution:
1. Verify JwtSettings__SecretKey is identical in all instances
2. Check token hasn't expired (TokenExpirationMinutes)
3. Ensure Issuer and Audience match between login and protected endpoints
```

### Migrations Fail on Startup
```
Solution:
1. Ensure database exists
2. Check user has CREATE/ALTER permissions
3. Test locally: dotnet ef database update
4. Run migrations manually in production console if needed
```

---

## Next Steps

1. **Choose Platform**: Railway (easiest) or Render (good balance) or Azure (most powerful)
2. **Create Account**: Sign up on platform website
3. **Generate Secret Key**: Use generator above
4. **Update Configuration**: 
   - Update appsettings.Production.json
   - Write down all environment variables
5. **Deploy Backend First**: Test with Postman before connecting frontend
6. **Deploy Frontend**: Update API URL in environment.prod.ts
7. **Monitor**: Watch logs for errors during first week
8. **Scale**: Upgrade plan if needed as usage grows

---

## Support Resources

- **Railway**: https://docs.railway.app
- **Render**: https://render.com/docs
- **Azure**: https://learn.microsoft.com/en-us/dotnet/azure/
- **.NET Configuration**: https://learn.microsoft.com/dotnet/core/runtime-config/
- **Entity Framework Migrations**: https://learn.microsoft.com/ef/core/managing-schemas/migrations/
- **JWT Authentication**: https://learn.microsoft.com/en-us/dotnet/api/microsoft.identitymodel.tokens.tokensheme

---

## Your Current Setup Details

- **Framework**: .NET 8 (Latest LTS)
- **Database**: PostgreSQL
- **Frontend**: Angular
- **Auth**: JWT
- **ORM**: Entity Framework Core

This is a modern, production-ready stack. Most deployment platforms have excellent support for it.
