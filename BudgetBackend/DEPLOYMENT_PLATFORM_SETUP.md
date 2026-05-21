# Platform-Specific Deployment Instructions

## 1. Railway Deployment (Recommended for Quick Start)

### Prerequisites:
- GitHub account with your code repository
- Railway account (https://railway.app)
- PostgreSQL database (Railway can create one for you)

### Step-by-Step:

#### 1.1 Prepare Your Repository
```bash
# Ensure your repository structure is:
# /BudgetBackend (ASP.NET Core project)
# /BudgetBackend.Web (or your Angular app)

# Add a .gitignore if not present:
echo "bin/
obj/
.vs/
*.user
*.suo" >> .gitignore

git add .
git commit -m "Prepare for Railway deployment"
git push
```

#### 1.2 Create Railway Project
1. Go to https://railway.app
2. Click "New Project" ? "Deploy from GitHub"
3. Select your repository
4. Choose the BudgetBackend (ASP.NET Core) directory as the root

#### 1.3 Configure Environment Variables
Railway will automatically detect it's a .NET app. Add these variables:

```
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=postgresql://<username>:<password>@<host>:<port>/<database>
JwtSettings__SecretKey=your-production-jwt-secret-key-minimum-32-characters-long
JwtSettings__Issuer=BudgetBackend
JwtSettings__Audience=BudgetApp
Cors__AllowedOrigins__0=https://your-angular-frontend.url
```

**Get PostgreSQL Connection String from Railway:**
- In Railway dashboard, click your project
- Select the PostgreSQL plugin
- Copy the connection string under "Database URL"
- Format: `postgresql://username:password@host:port/database`

#### 1.4 Configure Your Angular Frontend
In your Angular `environment.prod.ts`:
```typescript
export const environment = {
  production: true,
  apiUrl: 'https://your-backend-app.railway.app/api'
};
```

#### 1.5 Deploy
- Push code to GitHub
- Railway automatically deploys
- Monitor build logs in Railway dashboard
- App URL will be provided (e.g., `https://budgetbackend-prod.railway.app`)

---

## 2. Render Deployment

### Prerequisites:
- GitHub account
- Render account (https://render.com)

### Step-by-Step:

#### 2.1 Prepare Repository (same as Railway)

#### 2.2 Create Render Service
1. Go to https://render.com/dashboard
2. Click "New +" ? "Web Service"
3. Connect GitHub and select repository
4. Choose the BudgetBackend directory

#### 2.3 Configure Service
- **Name**: `budgetbackend` (or your choice)
- **Environment**: Select `.NET` or generic `Docker`
- **Build Command**: `dotnet publish -c Release -o out`
- **Start Command**: `dotnet ./out/BudgetBackend.dll`
- **Instance Type**: Free or Starter ($7/month)

#### 2.4 Add Environment Variables
```
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=your-postgresql-connection-string
JwtSettings__SecretKey=your-production-jwt-secret-key
JwtSettings__Issuer=BudgetBackend
JwtSettings__Audience=BudgetApp
Cors__AllowedOrigins__0=https://your-angular-frontend.url
```

#### 2.5 Create PostgreSQL Database (Render)
1. In Render dashboard, click "New +" ? "PostgreSQL"
2. Copy connection string
3. Update `ConnectionStrings__DefaultConnection` environment variable

#### 2.6 Deploy
- Push to GitHub
- Render automatically builds and deploys
- URL provided in dashboard (e.g., `https://budgetbackend.onrender.com`)

---

## 3. Azure Deployment (Most Powerful)

### Prerequisites:
- Azure account (free tier available: https://azure.microsoft.com/free)
- Azure CLI or Azure Portal
- GitHub account

### Step-by-Step (Using Portal):

#### 3.1 Create Azure App Service
1. Go to https://portal.azure.com
2. Click "Create a resource" ? "Web App"
3. **Subscription**: Your subscription
4. **Resource Group**: Create new (e.g., "BudgetApp-RG")
5. **Name**: `budgetbackend-prod` (must be globally unique)
6. **Publish**: Code
7. **Runtime Stack**: `.NET 8` or latest
8. **Operating System**: Linux (cheaper) or Windows
9. **App Service Plan**: Create new (Free or B1 tier: $15/month)
10. Click "Review + create" ? "Create"

#### 3.2 Create Azure Database for PostgreSQL
1. In Azure Portal, click "Create a resource" ? "Azure Database for PostgreSQL"
2. **Deployment option**: Single Server or Flexible Server
3. **Name**: `budgetdb`
4. **Server admin login**: `pgadmin`
5. **Password**: Strong password (save it!)
6. **Pricing tier**: Basic (free tier available)
7. **Firewall**: Click "Add current client IP"
   - Also add App Service IP (get from App Service networking settings)

#### 3.3 Get Database Connection String
1. Open PostgreSQL resource in Azure Portal
2. Go to "Connection strings"
3. Copy the connection string
4. Format: `Host=server.postgres.database.azure.com;Port=5432;Database=budget;Username=pgadmin@server;Password=PASSWORD;`

#### 3.4 Deploy Code to Azure

**Option A: GitHub Actions (Recommended)**

Create `.github/workflows/deploy.yml`:
```yaml
name: Deploy to Azure

on:
  push:
    branches: [main, develop]

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      
      - name: Setup .NET
        uses: actions/setup-dotnet@v3
        with:
          dotnet-version: '8.0.x'
      
      - name: Publish
        run: dotnet publish BudgetBackend/BudgetBackend.csproj -c Release -o ./publish
      
      - name: Deploy to Azure
        uses: azure/webapps-deploy@v2
        with:
          app-name: 'budgetbackend-prod'
          publish-profile: ${{ secrets.AZURE_PUBLISH_PROFILE }}
          package: ./publish
```

To get publish profile:
1. In Azure Portal, go to your App Service
2. Click "Download publish profile"
3. Go to GitHub ? Settings ? Secrets and variables ? Actions
4. Create `AZURE_PUBLISH_PROFILE` with the content from the profile file

#### 3.5 Configure Environment Variables (App Service)
1. Open App Service in Azure Portal
2. Go to "Configuration" ? "Application settings"
3. Add these variables:

| Name | Value |
|------|-------|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `ConnectionStrings__DefaultConnection` | Your PostgreSQL connection string |
| `JwtSettings__SecretKey` | Your production secret key |
| `JwtSettings__Issuer` | `BudgetBackend` |
| `JwtSettings__Audience` | `BudgetApp` |
| `Cors__AllowedOrigins__0` | `https://your-angular-app.com` |

#### 3.6 Configure CORS in Application Insights
Azure provides built-in monitoring and logging:
1. App Service ? "Application Insights" ? View Application Insights data
2. Monitor performance and errors

---

## 4. Connection String Formats

### PostgreSQL Connection String Examples

**Local (Development):**
```
Host=localhost;Port=5432;Database=Budget;Username=postgres;Password=12345678
```

**Railway:**
```
postgresql://username:password@container.railway.app:5432/budget
```

**Render:**
```
postgresql://user:password@dpg-xxxxx.postgres.render.com:5432/budgetdb
```

**Azure:**
```
Host=myserver.postgres.database.azure.com;Port=5432;Database=budget;Username=pgadmin@myserver;Password=mypassword;SSL Mode=Require;
```

---

## 5. Environment Variables Setup Reference

### For All Platforms - Required Variables:

```
ASPNETCORE_ENVIRONMENT=Production
ConnectionStrings__DefaultConnection=[YOUR_DB_CONNECTION_STRING]
JwtSettings__SecretKey=[STRONG_SECRET_KEY_MIN_32_CHARS]
JwtSettings__Issuer=BudgetBackend
JwtSettings__Audience=BudgetApp
Cors__AllowedOrigins__0=[YOUR_FRONTEND_URL]
Cors__AllowedOrigins__1=[OPTIONAL_SECOND_DOMAIN]
```

### Optional but Recommended:

```
Logging__LogLevel__Default=Information
Logging__LogLevel__Microsoft=Warning
```

---

## 6. Frontend Deployment Options

### Option A: Deploy Angular to Vercel (Recommended for Frontend)
1. Build: `ng build --configuration production`
2. Push to GitHub
3. Connect Vercel to GitHub repo
4. Vercel automatically builds and deploys
5. Update `Cors__AllowedOrigins` in backend with your Vercel domain

### Option B: Deploy to Netlify
1. Build: `ng build --configuration production`
2. Connect GitHub to Netlify
3. Build command: `npm run build`
4. Output: `dist/your-app-name`

### Option C: Same Server as Backend (Simpler)
Build Angular and serve from the same ASP.NET Core app:
1. Build Angular: `ng build --configuration production`
2. Copy contents of `dist` to `BudgetBackend/wwwroot`
3. In Program.cs, add: `app.UseStaticFiles();` before MapControllers
4. Deploy the whole thing together

---

## 7. Testing Your Deployment

### Health Check Endpoint:
```bash
curl https://your-backend-url/health
```

### Test API with JWT:
```bash
# 1. Get token
curl -X POST https://your-backend-url/api/admin/login \
  -H "Content-Type: application/json" \
  -d '{"username":"admin","password":"password"}'

# 2. Use token in subsequent requests
curl -H "Authorization: Bearer YOUR_TOKEN" \
  https://your-backend-url/api/societe
```

### Check Logs:
- **Railway**: Dashboard ? Logs tab
- **Render**: Logs section
- **Azure**: Application Insights ? Logs or Stream logs

---

## 8. Production Best Practices Checklist

- [ ] Update `appsettings.Production.json` with real domain
- [ ] Set strong JWT secret (use generator: https://generate-random.org)
- [ ] Enable HTTPS (all platforms do this automatically)
- [ ] Configure CORS for your frontend domain only
- [ ] Set up database backups (all platforms offer this)
- [ ] Monitor application logs and errors
- [ ] Set up alerts for downtime
- [ ] Use environment variables for all secrets (never hardcode)
- [ ] Test migration process in staging environment first
- [ ] Keep .NET and packages updated

---

## 9. Troubleshooting Common Issues

### Issue: "Access to XMLHttpRequest blocked by CORS"
**Solution**: Update `Cors__AllowedOrigins` in production config with your exact frontend URL

### Issue: "Connection refused to PostgreSQL"
**Solution**: 
1. Verify connection string format
2. Check firewall rules allow your app IP
3. Test connection from platform's console

### Issue: "Migrations failed"
**Solution**:
1. Check database user has proper permissions
2. Run migrations manually in platform console:
   ```bash
   dotnet ef database update
   ```

### Issue: "JWT token validation failed"
**Solution**: Ensure `JwtSettings__SecretKey` is the same in all environments

---

## Next Steps:

1. Choose your platform (Railway for simplicity, Azure for power)
2. Create account and read platform-specific docs
3. Update appsettings.Production.json
4. Set up environment variables
5. Deploy backend first, test with Postman
6. Deploy frontend and connect to backend URL
7. Monitor logs and adjust as needed

Good luck! ??
