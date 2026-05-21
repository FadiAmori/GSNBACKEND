using Microsoft.EntityFrameworkCore;
using Am.Infrastructure;
using Am.Infrastructure.Services;
using Am.ApplicationCore.Interfaces;
using Am.ApplicationCore.Services;
using BudgetBackend.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// Load environment-specific configuration
var environment = builder.Environment.EnvironmentName;
Console.WriteLine($"Starting application in {environment} mode");

// Add configuration files
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{environment}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// add configuration-based DbContext registration
builder.Services.AddDbContext<AmContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repository implementation
builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));

// Register JWT Service
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();

// Register all services
builder.Services.AddScoped<IServiceAdmin, ServiceAdmin>();
builder.Services.AddScoped<IServiceSociete, ServiceSociete>();
builder.Services.AddScoped<IServiceUserSociete, ServiceUserSociete>();
builder.Services.AddScoped<IServiceClesDeRepartition, ServiceClesDeRepartition>();
builder.Services.AddScoped<IServiceRapportFinancier, ServiceRapportFinancier>();
builder.Services.AddScoped<IServiceCategorieFinanciere, ServiceCategorieFinanciere>();
builder.Services.AddScoped<IServiceSousCategorieFinanciere, ServiceSousCategorieFinanciere>();
builder.Services.AddScoped<IServiceCategorieCR, ServiceCategorieCR>();
builder.Services.AddScoped<IServiceSousCategorieCR, ServiceSousCategorieCR>();
builder.Services.AddScoped<IServiceLigneFinanciere, ServiceLigneFinanciere>();
builder.Services.AddScoped<IServiceCR, ServiceCR>();
builder.Services.AddScoped<IServiceTypeClient, ServiceTypeClient>();
builder.Services.AddScoped<IServiceFamilleProduit, ServiceFamilleProduit>();
builder.Services.AddScoped<IServiceProduit, ServiceProduit>();
builder.Services.AddScoped<IServiceExcelVariable, ServiceExcelVariable>();
builder.Services.AddScoped<IServiceExcelLigneCalculee, ServiceExcelLigneCalculee>();
builder.Services.AddScoped<IServiceCBDash, ServiceCBDash>();
builder.Services.AddScoped<IServiceCountDash, ServiceCountDash>();
builder.Services.AddScoped<IServiceCourbDash, ServiceCourbDash>();
builder.Services.AddScoped<IServiceLigneCalculee, ServiceLigneCalculee>();
builder.Services.AddScoped<IServiceLigneFinanciere, ServiceLigneFinanciere>();

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = jwtSettings["SecretKey"] ?? "your-super-secret-key-that-is-at-least-32-characters-long-for-hs256";
var issuer = jwtSettings["Issuer"] ?? "BudgetBackend";
var audience = jwtSettings["Audience"] ?? "BudgetApp";

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
        };
    });

builder.Services.AddAuthorization();

// Add controllers
builder.Services.AddControllers();

// Configure CORS with environment-specific settings
var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>() 
    ?? new[] { "http://localhost:4200" };

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngular", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials());
});

// Add health checks
builder.Services.AddHealthChecks();

// Add Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// apply migrations at startup (development convenience)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AmContext>();
    try
    {
        db.Database.Migrate();
        Console.WriteLine("Migrations applied successfully.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Migration error: {ex.Message}");
        if (app.Environment.IsDevelopment())
            throw;
    }
}

Console.WriteLine($"Migrations applied (if any). Application started in {environment} mode.");

// Configure HTTP pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}
else
{
    // Production error handling
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseCors("AllowAngular");
app.UseAuthentication();
app.UseAuthorization();

// Add health check endpoint
app.MapHealthChecks("/health");

app.MapControllers();

app.Run();
