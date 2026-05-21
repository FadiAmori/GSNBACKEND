# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy project files
COPY ["BudgetBackend/BudgetBackend.csproj", "BudgetBackend/"]
COPY ["Am.ApplicationCore/Am.ApplicationCore.csproj", "Am.ApplicationCore/"]
COPY ["Am.Infrastructure/Am.Infrastructure.csproj", "Am.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "BudgetBackend/BudgetBackend.csproj"

# Copy remaining source code
COPY . .

# Build
WORKDIR "/src/BudgetBackend"
RUN dotnet build "BudgetBackend.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "BudgetBackend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Set environment
ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5000

EXPOSE 5000

HEALTHCHECK --interval=30s --timeout=3s --start-period=5s --retries=3 \
  CMD curl -f http://localhost:5000/health || exit 1

ENTRYPOINT ["dotnet", "BudgetBackend.dll"]
