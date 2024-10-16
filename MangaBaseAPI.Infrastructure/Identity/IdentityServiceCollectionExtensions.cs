using MangaBaseAPI.CrossCuttingConcerns.Identity;
using MangaBaseAPI.Domain.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace MangaBaseAPI.Infrastructure.Identity
{
    public static class IdentityServiceCollectionExtensions
    {
        public static IServiceCollection AddIdentityCore(this IServiceCollection services) 
        {
            services.AddIdentityCore<User>();

            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
