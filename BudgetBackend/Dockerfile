FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files
COPY BudgetBackend.csproj .
COPY ../Am.ApplicationCore/Am.ApplicationCore.csproj ../Am.ApplicationCore/
COPY ../Am.Infrastructure/Am.Infrastructure.csproj ../Am.Infrastructure/

# Restore
RUN dotnet restore

# Copy everything
COPY . .

# Publish
RUN dotnet publish -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 10000

ENTRYPOINT ["dotnet", "BudgetBackend.dll"]