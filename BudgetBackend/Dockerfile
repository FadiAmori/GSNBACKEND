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

# Publish
WORKDIR "/src/BudgetBackend"
RUN dotnet publish "BudgetBackend.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 10000

ENTRYPOINT ["dotnet", "BudgetBackend.dll"]
