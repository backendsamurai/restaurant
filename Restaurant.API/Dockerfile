# Restore and buid release version of app
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copy all files to workdir
COPY . .

# Restore dependencies
RUN dotnet restore

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