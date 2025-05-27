# Use the ASP.NET runtime image for the final container
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Use the .NET SDK image for building the app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy the csproj and restore as separate layers
COPY ["Certitrack/Certitrack.csproj", "Certitrack/"]
RUN dotnet restore "Certitrack/Certitrack.csproj"

# Copy the rest of the source code
COPY . .
WORKDIR "/src/Certitrack"

# Build and publish the app
RUN dotnet publish "Certitrack.csproj" -c Release -o /app/publish

# Build runtime image
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Certitrack.dll"]
