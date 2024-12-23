using Microsoft.Extensions.DependencyInjection;

namespace MangaBaseAPI.Infrastructure.Authorization
{
    public static class AuthorizationSetupCollectionExtensions
    {
        public static IServiceCollection AddAuthorizationPolicies(this IServiceCollection services)
        {
            services.AddAuthorization();
            services.ConfigureOptions<AuthorizationOptionsSetup>();

            return services;
        }
    }
}
