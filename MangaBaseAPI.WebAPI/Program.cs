using MangaBaseAPI.WebAPI;
using MangaBaseAPI.Persistence;
using MangaBaseAPI.Application;
using MangaBaseAPI.Infrastructure.Identity;
using MangaBaseAPI.Infrastructure.Jwt;
using MangaBaseAPI.Infrastructure.Swagger;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
{
    // Web API
    builder.Services
        .AddWebApi()
        .AddGlobalErrorHandling();

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
        .AddSwagger();
}

var app = builder.Build();
{
    app.RegisterPipelines()
        .AddSwagger()
        .UseGlobalErrorHandling();

    app.Run();
}
