# MangaBaseAPI

**MangaBaseAPI** is a Web API built with ASP.NET Core using Clean architecture and CQRS principles. It allows users to **upload**, **read**, and **manage** digital comics (manga) with robust support for background processing, caching, and third-party services.

## Features

- **User Authentication and Authorization** (ASP.NET Identity + JWT)
- **Manga Upload & Management**
- **Manga Reading API**
- **User Account & Profile Management**
- **Caching with Redis**
- **Email Notification with Google Email Service**
- **Background Job Scheduling with Hangfire**
- **Google Cloud Storage Integration**
- **API Versioning**

## Technologies Used

- **ASP .NET Core Web API** (.NET 8)
- **Entity Framwork Core**
- **SQL Server** (Local & Azure Cloud)
- **Redis Cache** (via IDistributedCache)
- **Google Cloud Platform** (Email, storage)
- **Azure Cloud** (Deployment via Azure App Service)
- **Docker**
- **Hangfire** (Background job scheduling)
- **MediatR** (For implementing CQRS, applying pipeline behaviors)
- **AutoMapper** (Object mapping)
- **FluentValidation** (Request validating)

## Architecture & Design Patterns

- **Clean Architecture:** The solution follow Clean architecture with the following projects structure:

  - **Domain:** Focus on domain logic
  - **Application:** Focus on business logic
  - **Persistence:** Integrating external database service (Entity configurations, migrations,...)
  - **Infrastructure:** Intergrating external services (Caching, email, authentication,...)
  - **CrossCuttingConcerns:** Abstraction layer for external services
  - **WebAPI:** Presentation layer with ASP .NET Core Web API

- **Dependency Injection Pattern:** Injecting services via ASP .NET Core built-in service container
- **CQRS Pattern:** Implementing command and query seperation, request processing pipeline
- **Repository Pattern:** Intermidiate layer between application and data access layer
- **Unit of Work Pattern:** Manage database transaction and ensure data consistency between repository and data access layer
- **Result Pattern:** Defined generic result structure, enable robust result processing when success or fail.
- **Specification Pattern:**

## Getting Started

### Prerequisites

- .NET 8 SDK
- SQL Server
- Redis
- Google Cloud Service (Email, Storage)

### Local Setup

#### Clone the repository

```
git clone https://github.com/HoaNT3010/MangaBaseAPI.git
cd MangaBaseAPI.WebAPI
```

#### Google Cloud Service

You need to setup you Google Cloud Project and setup services beforehand. This included:

- Google Cloud Service Account
- Google Cloud Storage Bucket
- Create and store the Google Cloud Project credentials to you local device

You can find detailed instructions on how to setup Google Cloud Service at https://cloud.google.com/apis/docs/getting-started

#### Secrets and configuration

Use User Secrets for local development:

```
dotnet user-secrets init
```

Add application secrets via User Secrets:

```
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "YourDbConnectionString"
dotnet user-secrets set "ConnectionStrings:HangfireDB" "YourHangFireDbConnectionString"
dotnet user-secrets set "Jwt:SecretKey" "JwtSecretKey"
dotnet user-secrets set "GmailEmail:UserName" "YourGmailUserName"
dotnet user-secrets set "GmailEmail:Password" "YourGmailPassword"
dotnet user-secrets set "GmailEmail:FromEmail" "YourGmailEmailSender"
```

Note: You can configure HangFire to use the same database with your Web API application or using another separate database.

#### Appsettings configuration

Additional application settings can be configure via appsettings.json files. This included:

- **appsettings.json:** General application settings/configurations.
- **appsettings.Development.json:** Settings/configurations applied to Development environment.
- **appsettings.Production.json:** Settings/configurations applied to Production environment.

Some settings you need to change are:

- Redis: Change it according to your Redis service settings:

```
"Redis": {
    "Configuration": "YourRedisHost",
    "InstanceName": "YourRedisInstanceName"
  }
```

- Google Cloud Storage: Change it according to your Google Cloud Storage Bucket:

```
"GoogleCloudStorage": {
    "BucketName": "YourGoogleCloudStorageBucketName"
  }
```

#### Apply database migrations (If needed)

```
dotnet ef database update -p MangaBaseAPI.Persistence -s MangaBaseAPI.WebAPI
```

#### Run the API

```
dotnet run --project MangaBaseAPI.WebAPI
```

By default, the API should be available at

- http://localhost:8080
- https://localhost:8081

You can access the Swagger documentation at: https://localhost:8081/swagger/index.html

You can try health check endpoint at: https://localhost:8081/health

### Local Setup with Docker

#### General

This project includes a docker-compose.yml file to help you quickly spin up the application along with all required services (Redis, SQL Server, etc.) using Docker.

Typical Docker Compose services included:

- **MangaBaseAPI.WebAPI:** Main Web API application
- **SQL Server:** Local database instance
- **Redis:** Caching service

For SQL Server and Redis services, you can use external instances or with instances from Docker images (The API use external SQL Server and Redis services by default, not with Docker images).

You need to configure the SQL Server and Redis services in the docker-compose.yml file if you wish to use them as Docker images version.

#### Configure environment variables

You can edit the **docker-compose.override.yml** file or create new .env file to configure settings for Docker. The settings should be configured are:

- Database connection strings
- JWT Secret key
- Redis configuration
- Google Cloud Services
- Google Cloud credential file path

#### Run with Docker Compose

```
docker-compose up --build
```

This will build the Web API image, connect to or start Redis, SQL Server.

By default, the API should be available at

- http://localhost:8080
- https://localhost:8081

You can access the Swagger documentation at: https://localhost:8081/swagger/index.html

You can try health check endpoint at: https://localhost:8081/health

#### Running migrations in Docker (Optional)

If you using Docker image version of SQL Server database, you can automatically apply migration by adding this code to the Program.cs file when configuration the application:

```
using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<MangaBaseDbContext>();
                try
                {
                    db.Database.Migrate();
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error when trying to apply migration to database: " + ex.Message);
                }
            }
```

Or you can run it manually:

```
docker exec -it <web_api_container_name> bash
dotnet ef database update -p MangaBaseAPI.Persistence -s MangaBaseAPI.WebAPI
```

#### Stop and clean up

You can stop the application by running this command:

```
docker-compose down
```

Additionally, you can use this command to remove attached volumes (Redis/SQL Server data):

```
docker-conpose down --volumes
```

### Cloud Deployment

The API is deployed on Azure Cloud platform via Azure App Service with code deployment.

External services such as SQL Server, Redis,... are connected from cloud providers.

You can view the API at: https://mangabase-webapi.azurewebsites.net (Note: The API and services may take some time to start)

You can access the deployed API Swagger documentation at: https://mangabase-webapi.azurewebsites.net/swagger/index.html

## Testing

- Unit testing with **xUnit**
- Services mocking with **NSubstitute**
- Fluent assertions with **FluentAssertions**

You can run unit tests with command:

```
dotnet test MangaBaseAPI.UnitTests
```

## Acknowledgments

- Milan Jovanović - [Youtube](https://www.youtube.com/@MilanJovanovicTech)
- Amichai Mantinband - [Youtube](https://www.youtube.com/@amantinband)
