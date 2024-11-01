# Restaurant Pet Project on NET 8 (inspired by [itstep-Restoran](https://github.com/itstep-sabatex/itstep-Restoran))

## Which Technologies uses this project:

- **PostgreSQL** (as a primary database)
- **Redis** (for caching most requested data)
- **EntityFrameworkCore** (.NET library for accessing data from database as .NET objects - ORM)
- **Serilog** (.NET library for more structured logging)
- **MassTransit** (.NET library for abstractions over message brokers. The project uses a queue in memory)
- **FluentValidation** (.NET library for validation models)
- **FluentEmail** (.NET library for sending email with Razor Template Engine)
- **Redis.OM** (.NET library for use Redis with .NET built-in collection types)
- **Mapster** (.NET library for mapping objects)
- **Humanizer** (.NET library for converting strings,numbers, etc. to human readable format)

## Roadmap (future updates)

- Move controllers to library (**Restaurant.API.Presentation**) and rewrite using minimal api
- Move entities to domain library (**Restaurant.API.Domain**) and convert into aggregates with value object
- Move services call into **MediatR** handlers
- Move entity configurations, mail, messaging and other to infrastructure layer (**Restaurant.API.Infrastructure**)
- Add more parameters for sorting, filtering (by date for example) to API routes

## How to start

- With docker
  ```
  docker-compose up -d
  ```
- With dotnet
  ```
  dotnet run
  ```

## How to stop and cleanup

- With docker
  ```
  docker-compose down --rmi='local'
  ```
- With dotnet
  ```
  Press Ctrl+C to exit
  ```
