# Use the official .NET SDK image for building
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy the csproj and restore dependencies
COPY ["Certitrack.csproj", "."]
RUN dotnet restore "Certitrack.csproj"

# Copy everything else and build
COPY . .
RUN dotnet publish "Certitrack.csproj" -c Release -o /app/publish

# Use ASP.NET runtime image for final build
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Expose the default port
EXPOSE 80

# Set entrypoint
ENTRYPOINT ["dotnet", "Certitrack.dll"]
