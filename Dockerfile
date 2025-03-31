# Restore and buid release version of app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy all files to workdir
COPY ./Restaurant.API ./Restaurant.API
COPY ./Restaurant.Application ./Restaurant.Application
COPY ./Restaurant.Domain ./Restaurant.Domain
COPY ./Restaurant.Infrastructure ./Restaurant.Infrastructure
COPY ./Restaurant.Shared ./Restaurant.Shared
COPY ./Restaurant.Services ./Restaurant.Services
COPY ./Restaurant.Persistence ./Restaurant.Persistence
COPY ./restaurant.sln .

# Change directory to startup project
WORKDIR /app/Restaurant.API

# Restore dependencies
RUN dotnet restore

# Change directory to build stage
WORKDIR /app

# build release version of app
RUN dotnet publish -c Release -o bin

# Run release version of app from build stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# Copy dll file from build stage to workdir of current stage
COPY --from=build /app/bin .

# Expose container port for app
EXPOSE 8080

# Run app
ENTRYPOINT [ "dotnet", "Restaurant.API.dll" ]
