# =============================================================================
# Stage 1: BASE — Runtime image
# =============================================================================
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app

# Expose HTTP and HTTPS ports
EXPOSE 80
EXPOSE 443

# =============================================================================
# Stage 2: BUILD — SDK image for compiling the application
# =============================================================================
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src

# Copy all .csproj files first (layer-cache friendly — only re-runs restore when .csproj changes)
COPY ["HospitalManagementSystems.API/HospitalManagementSystems.API.csproj",               "HospitalManagementSystems.API/"]
COPY ["HospitalManagementSystem.Services/HospitalManagementSystem.Services.csproj",        "HospitalManagementSystem.Services/"]
COPY ["HospitalManagementSystem.Models/HospitalManagementSystem.Models.csproj",            "HospitalManagementSystem.Models/"]
COPY ["HospitalManagementSystem.Data/HospitalManagementSystem.DBContext.csproj",           "HospitalManagementSystem.Data/"]
COPY ["HospitalManagementSystem.Shared/HospitalManagementSystem.Shared.csproj",            "HospitalManagementSystem.Shared/"]

# Restore NuGet packages
RUN dotnet restore "HospitalManagementSystems.API/HospitalManagementSystems.API.csproj"

# Copy the rest of the source code
COPY . .

# Build in Release mode
WORKDIR "/src/HospitalManagementSystems.API"
RUN dotnet build "HospitalManagementSystems.API.csproj" -c Release -o /app/build

# =============================================================================
# Stage 3: PUBLISH — Create the final deployable output
# =============================================================================
FROM build AS publish
RUN dotnet publish "HospitalManagementSystems.API.csproj" \
    -c Release \
    -o /app/publish \
    --no-restore \
    /p:UseAppHost=false

# =============================================================================
# Stage 4: FINAL — Lean production image
# =============================================================================
FROM base AS final
WORKDIR /app

# Create a non-root user for security best practice
RUN addgroup --system appgroup && adduser --system --ingroup appgroup appuser

# Copy published output from publish stage
COPY --from=publish /app/publish .

# Set ownership to non-root user
RUN chown -R appuser:appgroup /app

USER appuser

# Health check — pings the Swagger/health endpoint every 30s
HEALTHCHECK --interval=30s --timeout=10s --start-period=30s --retries=3 \
  CMD curl -f http://localhost:80/health || exit 1

# Entry point
ENTRYPOINT ["dotnet", "HospitalManagementSystems.API.dll"]
