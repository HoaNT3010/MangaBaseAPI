using MangaBaseAPI.CrossCuttingConcerns.Jwt;
using Microsoft.Extensions.DependencyInjection;

namespace MangaBaseAPI.Infrastructure.Jwt
{
    public static class JwtServiceCollectionExtensions
    {
        public static IServiceCollection AddJwtProvider(this IServiceCollection services)
        {
            services.AddAuthentication()
                .AddJwtBearer();
            services.ConfigureOptions<JwtOptionsSetup>();
            services.ConfigureOptions<JwtBearerOptionsSetup>();
            services.ConfigureOptions<AuthenticationOptionsSetup>();

            services.AddScoped<IJwtTokenProvider, JwtTokenProvider>();

            return services;
        }
    }
}
