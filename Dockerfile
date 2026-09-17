# ==========================================
# BUILD STAGE
# ==========================================
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

# Copy solution file
COPY PayrollApi.slnx ./

# Copy project files first
# This allows Docker to cache the restore layer
COPY Application/Application.csproj Application/
COPY Domain/Domain.csproj Domain/
COPY Infrastructure/Infrastructure.csproj Infrastructure/
COPY Payroll/Payroll.csproj Payroll/

# Restore dependencies
RUN dotnet restore Payroll/Payroll.csproj

# Copy the remaining source code
COPY . .

# Publish the application
RUN dotnet publish Payroll/Payroll.csproj \
    -c Release \
    -o /app/publish \
    --no-restore


# ==========================================
# RUNTIME STAGE
# ==========================================
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final

WORKDIR /app

# Copy published application
COPY --from=build /app/publish .

# ASP.NET Core 10 container port
ENV ASPNETCORE_HTTP_PORTS=8080

# Document the port
EXPOSE 8080

# Start application
ENTRYPOINT ["dotnet", "Payroll.dll"]