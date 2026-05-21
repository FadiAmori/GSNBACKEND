# Complete Deployment Guide: ASP.NET Core + Angular + PostgreSQL

## Executive Summary

Your tech stack:
- **Backend**: ASP.NET Core 8 (BudgetBackend)
- **Frontend**: Angular (separate app)
- **Database**: PostgreSQL
- **Authentication**: JWT

**Best Deployment Option for Your Setup**: **Railway** (fastest setup) or **Azure** (most powerful)

### Why Not Vercel for Backend?
Vercel is optimized for Node.js and static sites. For .NET backends, you need a platform that supports the CLR (Common Language Runtime). Vercel doesn't have .NET runtime support.

---

## Part 1: Understanding Your Options

### ?? **Railway** - Best for Getting Started Fast
- **Time to deploy**: 5 minutes
- **Monthly cost**: $5-20
- **Best for**: MVPs, prototypes, small-medium production apps

**Why choose Railway?**
1. One-click GitHub integration
2. PostgreSQL included
3. Super simple environment variables
4. Automatic HTTPS
5. Good free tier to test

**Drawback**: Limited customization, moderate scaling features

---

### ?? **Render** - Best Middle Ground
- **Time to deploy**: 10 minutes
- **Monthly cost**: $7-30
- **Best for**: Small to medium production apps

**Why choose Render?**
1. Easy GitHub integration
2. Built-in PostgreSQL
3. More control than Railway
4. Good monitoring
5. Generous free tier

**Drawback**: Colder starts on free tier, fewer advanced features

---

### ?? **Azure** - Best for Enterprise
- **Time to deploy**: 15 minutes
- **Monthly cost**: $10-50 (free tier 12 months)
- **Best for**: Enterprise apps, heavy scaling needs

**Why choose Azure?**
1. Native .NET integration
2. Excellent monitoring and logging
3. Auto-scaling capabilities
4. Managed PostgreSQL service
5. Microsoft support
6. Integration with other services

**Drawback**: Slightly more complex, more moving parts

---

## Part 2: What Changed in Your Code

### Updated `Program.cs`
? Added environment-specific configuration loading  
? Added CORS configuration from appsettings  
? Added health check endpoint (`/health`)  
? Added error handling for production vs development  
? Improved logging for startup

### New Configuration Files
? `appsettings.Production.json` - Production-specific settings  
? Updated `appsettings.json` - Added Cors and Logging sections

### New Infrastructure Files
? `Dockerfile` - Container configuration for any platform  
? `docker-compose.yml` - Local testing with Docker  
? Deployment guides and documentation

---

## Part 3: Step-by-Step Deployment (Choose One)

### **FASTEST: Railway Deployment (Recommended)**

#### Step 1: Prepare Your Code
```bash
# In your BudgetBackend repository root
git status
git add .
git commit -m "Add production configuration and deployment files"
git push origin main
```

#### Step 2: Create Railway Account & Project
1. Go to https://railway.app
2. Sign up with GitHub
3. Click "New Project"
4. Select "Deploy from GitHub repo"
5. Choose your repository
6. Select the `BudgetBackend` directory as root (if prompted)

#### Step 3: Configure Environment Variables
In Railway dashboard, go to your service ? Variables, add:

```env
ASPNETCORE_ENVIRONMENT=Production

# PostgreSQL Connection String
# Format: postgresql://username:password@host:port/database
ConnectionStrings__DefaultConnection=postgresql://root:password@rail-host:5432/budget

# JWT Settings - Change these!
JwtSettings__SecretKey=your-STRONG-secret-key-MINIMUM-32-characters-GENERATE-NEW
JwtSettings__Issuer=BudgetBackend
JwtSettings__Audience=BudgetApp

# CORS - Update with your Angular app domain
Cors__AllowedOrigins__0=https://your-angular-app.vercel.app
```

#### Step 4: Add PostgreSQL Plugin
1. In Railway project, click "Add a service"
2. Select "PostgreSQL"
3. Railway automatically creates the database
4. Copy the connection string from PostgreSQL service details
5. Update `ConnectionStrings__DefaultConnection` variable

#### Step 5: Deploy
```bash
# Just push to GitHub - Railway deploys automatically!
git push origin main
```

Railway builds and deploys. You'll see your app URL in the dashboard (e.g., `https://budgetbackend-prod.railway.app`)

#### Step 6: Verify Deployment
```bash
# Check health
curl https://your-railway-url/health

# Should return: HTTP 200 OK
```

#### Step 7: Connect Angular Frontend
In your Angular app's `environment.prod.ts`:
```typescript
export const environment = {
  production: true,
  apiUrl: 'https://your-railway-url/api'  // Update this URL!
};
```

Then deploy your Angular frontend to Vercel as usual.

---

### ALTERNATIVE: Render Deployment

#### Step 1-2: Same as Railway (GitHub push)

#### Step 3: Create Render Service
1. Go to https://render.com
2. Sign up with GitHub
3. Click "New +" ? "Web Service"
4. Select your repository
5. Choose `BudgetBackend` directory

#### Step 4: Configure Service
- **Name**: `budgetbackend`
- **Environment**: Select language `.NET`
- **Build Command**: `dotnet publish -c Release -o out`
- **Start Command**: `dotnet ./out/BudgetBackend.dll`
- **Instance Type**: Free or Starter

#### Step 5: Add Environment Variables
Same as Railway (see Part 3 Step 3)

#### Step 6: Add PostgreSQL
1. Click "New +" ? "PostgreSQL"
2. Name: `budgetdb`
3. Copy connection string to environment variables
4. Create service

#### Step 7-8: Verify and Connect
Same as Railway (Steps 6-7)

---

### ENTERPRISE: Azure Deployment

#### Step 1: Create Azure Resources

**Create App Service:**
1. Go to https://portal.azure.com
2. "Create a resource" ? "Web App"
3. Fill in:
   - Name: `budgetbackend-prod`
   - Runtime: `.NET 8`
   - Region: Choose closest to users
   - App Service Plan: Create new (Free or B1)

**Create PostgreSQL Database:**
1. "Create a resource" ? "Azure Database for PostgreSQL"
2. Single server or Flexible Server
3. Configure admin credentials
4. Remember: `server-name`, `admin-name`, `password`

#### Step 2: Get Connection String
In Azure Portal, PostgreSQL ? "Connection strings"
Copy and save the connection string.

#### Step 3: Configure App Service Variables
In App Service ? Configuration ? Application settings:

| Setting | Value |
|---------|-------|
| ASPNETCORE_ENVIRONMENT | Production |
| ConnectionStrings__DefaultConnection | [Your PostgreSQL connection string] |
| JwtSettings__SecretKey | [Strong key] |
| JwtSettings__Issuer | BudgetBackend |
| JwtSettings__Audience | BudgetApp |
| Cors__AllowedOrigins__0 | [Your frontend URL] |

#### Step 4: Deploy Code
Create `.github/workflows/azure-deploy.yml`:
```yaml
name: Deploy to Azure

on:
  push:
    branches: [main]

jobs:
  deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - uses: azure/webapps-deploy@v2
        with:
          app-name: 'budgetbackend-prod'
          publish-profile: ${{ secrets.AZURE_PUBLISH_PROFILE }}
          package: ./publish
```

Get publish profile:
1. App Service ? "Download publish profile"
2. GitHub ? Settings ? Secrets ? `AZURE_PUBLISH_PROFILE`
3. Paste profile content

Then push to GitHub - Azure deploys automatically!

---

## Part 4: PostgreSQL Connection Strings

### Format Reference
```
postgresql://[username]:[password]@[host]:[port]/[database]
```

### Examples by Platform

**Railway:**
```
postgresql://root:PASSWORD@containers.railway.app:5432/railway
```

**Render:**
```
postgresql://user:PASSWORD@dpg-xxxxx.postgres.render.com:5432/database
```

**Azure:**
```
Host=myserver.postgres.database.azure.com;Port=5432;Database=budget;Username=pgadmin@myserver;Password=PASSWORD;SSL Mode=Require;
```

**Docker (Local):**
```
Host=postgres;Port=5432;Database=Budget;Username=postgres;Password=budgetapp123!
```

---

## Part 5: Testing Your Deployment

### 1. Health Check
```bash
curl https://your-backend-url/health
# Should return: 200 OK
```

### 2. Get JWT Token
```bash
curl -X POST https://your-backend-url/api/admin/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"yourpassword"}'

# Should return JSON with "token" field
```

### 3. Use Token to Access Protected Endpoint
```bash
curl -H "Authorization: Bearer YOUR_TOKEN_HERE" \
  https://your-backend-url/api/societe

# Should return data (not 401 Unauthorized)
```

### 4. Test CORS from Frontend
In browser console on your Angular app:
```javascript
fetch('https://your-backend-url/api/societe', {
  headers: { 'Authorization': 'Bearer YOUR_TOKEN' }
}).then(r => r.json()).then(console.log)
```

Should work without CORS errors. If CORS errors appear:
1. Ensure `Cors__AllowedOrigins__0` matches your exact frontend URL
2. No trailing slashes: `https://example.com` not `https://example.com/`
3. Include protocol: `https://` not just `example.com`

---

## Part 6: Configuration Files to Know

### Files You Just Updated/Created:

1. **`Program.cs`** - Main application startup
   - Environment-aware configuration
   - CORS from appsettings
   - Health check endpoint

2. **`appsettings.json`** - Shared across all environments
   ```json
   {
     "ConnectionStrings": { "DefaultConnection": "..." },
     "JwtSettings": { ... },
     "Cors": { "AllowedOrigins": ["..."] }
   }
   ```

3. **`appsettings.Production.json`** - Production overrides
   - Real frontend URL
   - Production database
   - Strong JWT secret

4. **`Dockerfile`** - Container image for any platform
   - Multi-stage build (smaller images)
   - Health checks included

5. **`docker-compose.yml`** - Local testing
   - PostgreSQL + App in Docker
   - Use: `docker compose up`

---

## Part 7: Security Checklist

Before deploying to production:

- [ ] **JWT Secret**: Generated strong key (minimum 32 chars)
- [ ] **Database Password**: Not default, strong password
- [ ] **CORS Origin**: Set to actual frontend domain (not localhost)
- [ ] **HTTPS**: Enabled (all platforms do this by default)
- [ ] **Secrets**: All stored in environment variables, NOT in code
- [ ] **Database Backup**: Enabled on hosting platform
- [ ] **Logging**: Configured for production (not too verbose)
- [ ] **SSL Mode**: `Require` in PostgreSQL connection string

**Never commit to GitHub:**
- Database passwords
- JWT secrets
- API keys
- Connection strings with real credentials

---

## Part 8: Costs Summary

### Railway
- PostgreSQL database: Free or $5/month
- App service: Free or $5-20/month
- **Total**: $5-25/month

### Render
- PostgreSQL database: Free or $15/month
- App service: Free or $7-15/month
- **Total**: $7-30/month

### Azure
- App Service (Free tier first 12 months, then ~$15/month)
- PostgreSQL database: Free tier available
- **Total**: Free (12 months), then $15-50/month

---

## Part 9: Monitoring & Maintenance

### What to Monitor:
1. **Error logs** - Check daily first week
2. **Database connections** - Ensure not maxed out
3. **API response times** - Watch for slowdowns
4. **Disk space** - PostgreSQL logs can grow
5. **Resource usage** - CPU, memory

### How to Check:
- **Railway**: Dashboard ? Logs tab
- **Render**: Service ? Logs
- **Azure**: Application Insights or App Service logs

### Common Issues:

**CORS Error in Browser**
```
Access to XMLHttpRequest blocked by CORS
```
Solution: Update `Cors__AllowedOrigins__0` with correct frontend URL

**JWT Token Invalid**
```
401 Unauthorized
```
Solution: Ensure `JwtSettings__SecretKey` is consistent

**Database Connection Failed**
```
NpgsqlException: could not translate host name "host"
```
Solution: Verify connection string format and firewall rules

---

## Part 10: Next Steps (In Order)

1. ? **Code Changes Done** - Program.cs updated, configuration files created
2. **Choose Platform** - Railway (easiest) or Azure (most powerful)
3. **Create Account** - Sign up on chosen platform
4. **Generate JWT Secret** - Use: `openssl rand -base64 32`
5. **Deploy Backend** - Follow platform steps above
6. **Test Backend** - Use curl commands in Part 5
7. **Update Frontend** - Update `environment.prod.ts`
8. **Deploy Frontend** - Deploy Angular to Vercel/Netlify
9. **Monitor** - Watch logs first week
10. **Scale** - Upgrade plan if needed

---

## Part 11: Quick Start Commands

### Local Docker Testing (Before Cloud Deployment)
```bash
# Start local environment
docker compose up

# Runs backend at: http://localhost:5000
# Runs PostgreSQL at: localhost:5432

# In another terminal, test
curl http://localhost:5000/health

# Stop when done
docker compose down
```

### Generate Strong Keys
```bash
# PowerShell
[Convert]::ToBase64String((1..64 | ForEach-Object { [byte](Get-Random -Max 256) }) )

# Bash/Linux
openssl rand -base64 32
```

### Deploy to Git (Trigger Cloud Deploy)
```bash
git add .
git commit -m "Deploy to production"
git push origin main

# Platform automatically builds and deploys!
```

---

## Final Checklist

Before going live:

- [ ] Chose a deployment platform
- [ ] Created account on platform
- [ ] Generated production JWT secret
- [ ] Updated `appsettings.Production.json`
- [ ] Created/configured PostgreSQL database
- [ ] Set all environment variables on platform
- [ ] Code pushed to GitHub
- [ ] Backend deployed and health check working
- [ ] Database migrations ran successfully
- [ ] Tested API with Postman/curl
- [ ] Updated Angular `environment.prod.ts`
- [ ] Frontend deployed and pointing to correct API
- [ ] Tested full login flow (frontend ? backend)
- [ ] Enabled database backups
- [ ] Set up error monitoring/logging

---

## Support & Resources

- **Railway Docs**: https://docs.railway.app
- **Render Docs**: https://render.com/docs
- **Azure Docs**: https://learn.microsoft.com/en-us/dotnet/azure/
- **.NET Configuration**: https://learn.microsoft.com/dotnet/core/configuration/
- **PostgreSQL Connection Strings**: https://www.postgresql.org/docs/current/libpq-connect.html
- **JWT Best Practices**: https://tools.ietf.org/html/rfc8725

---

## Questions?

Check the other deployment guides in your project:
- `DEPLOYMENT_PLATFORM_SETUP.md` - Detailed per-platform instructions
- `DEPLOYMENT_QUICK_REFERENCE.md` - Quick lookup table
- `.github/workflows/` - CI/CD examples (if created)

Good luck with your deployment! ??
