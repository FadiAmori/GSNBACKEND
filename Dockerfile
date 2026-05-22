FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy csproj files
COPY BudgetBackend/BudgetBackend.csproj BudgetBackend/
COPY Am.ApplicationCore/Am.ApplicationCore.csproj Am.ApplicationCore/
COPY Am.Infrastructure/Am.Infrastructure.csproj Am.Infrastructure/

# Restore dependencies
RUN dotnet restore "BudgetBackend/BudgetBackend.csproj"

# Copy all source code
COPY . .

# Publish application
WORKDIR /src/BudgetBackend
RUN dotnet publish "BudgetBackend.csproj" -c Release -o /app/publish

# Runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 10000

ENTRYPOINT ["dotnet", "BudgetBackend.dll"]