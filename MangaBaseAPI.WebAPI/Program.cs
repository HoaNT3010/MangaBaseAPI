using MangaBaseAPI.WebAPI;
using MangaBaseAPI.Persistence;
using MangaBaseAPI.Application;
using MangaBaseAPI.Infrastructure.Identity;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
{
    builder.Services.AddWebApi()
    .AddPersistence(connectionString!)
    .AddApplication()
    .AddIdentityCore();
}

var app = builder.Build();
{
    app.AddSwagger();

    app.RegisterPipelines();

    app.Run();
}
