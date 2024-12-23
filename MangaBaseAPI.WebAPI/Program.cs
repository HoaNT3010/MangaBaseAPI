using MangaBaseAPI.WebAPI;
using MangaBaseAPI.Persistence;
using MangaBaseAPI.Application;
using MangaBaseAPI.Infrastructure.Identity;
using MangaBaseAPI.Infrastructure.Jwt;
using MangaBaseAPI.Infrastructure.Swagger;
using Serilog;
using MangaBaseAPI.Infrastructure.Caching.Redis;
using MangaBaseAPI.Infrastructure.Storage;
using MangaBaseAPI.Infrastructure.Authorization;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
{
    // Web API
    builder.Services
        .AddWebApi()
        .AddGlobalErrorHandling();

    // Serilog
    Log.Logger = new LoggerConfiguration()
        .ReadFrom.Configuration(builder.Configuration)
        .CreateLogger();
    builder.Host.UseSerilog(Log.Logger);

    // Application
    builder.Services
        .AddApplication();

    // Persistence
    builder.Services
        .AddPersistence(connectionString!);

    // Infrastructure
    builder.Services
        .AddIdentityCore()
        .AddJwtProvider()
        .AddAuthorizationPolicies()
        .AddSwagger()
        .AddRedisCaching(builder.Configuration)
        .AddStorageServices();
}

var app = builder.Build();
{
    app.RegisterPipelines()
        .AddSwagger()
        .UseGlobalErrorHandling()
        .RegisterMiddlewares();

    app.Run();
}
