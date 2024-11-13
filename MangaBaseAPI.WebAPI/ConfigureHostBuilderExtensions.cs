using Serilog;

namespace MangaBaseAPI.WebAPI
{
    public static class ConfigureHostBuilderExtensions
    {
        public static ConfigureHostBuilder ConfigureWebApi(this ConfigureHostBuilder builder)
        {
            builder.UseSerilog((context, configuration) =>
            {
                configuration.ReadFrom.Configuration(context.Configuration);
            });

            return builder;
        }
    }
}
