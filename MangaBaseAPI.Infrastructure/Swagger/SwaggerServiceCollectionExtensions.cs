using Microsoft.Extensions.DependencyInjection;

namespace MangaBaseAPI.Infrastructure.Swagger
{
    public static class SwaggerServiceCollectionExtensions
    {
        public static IServiceCollection AddSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen();
            services.ConfigureOptions<SwaggerOptionsSetup>();

            return services;
        }
    }
}
