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
using MangaBaseAPI.Infrastructure.BackgroundJob.HangfireScheduler;
using MangaBaseAPI.Infrastructure.Email.Gmail;

var builder = WebApplication.CreateBuilder(args);
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
        .AddPersistence(builder.Configuration, builder.Environment.EnvironmentName);

    // Infrastructure
    builder.Services
        .AddIdentityCore()
        .AddJwtProvider()
        .AddAuthorizationPolicies()
        .AddSwagger()
        .AddRedisCaching(builder.Configuration, builder.Environment.EnvironmentName)
        .AddStorageServices(builder.Environment.EnvironmentName)
        .AddHangfireServices(builder.Configuration, builder.Environment.EnvironmentName)
        .AddGmailEmailService(builder.Configuration);
}

var app = builder.Build();
{
    app.RegisterPipelines()
        .AddSwagger()
        .UseGlobalErrorHandling()
        .RegisterMiddlewares()
        .AddHangfireDashboard()
        .UseHealthChecks();


    app.Run();
}
