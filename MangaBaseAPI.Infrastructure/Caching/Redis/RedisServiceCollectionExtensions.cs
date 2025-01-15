using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MangaBaseAPI.Infrastructure.Caching.Redis
{
    public static class RedisServiceCollectionExtensions
    {
        const string RedisSection = "Redis";
        const string RedisInstanceNameSection = "InstanceName";
        const string RedisConfigurationSection = "Configuration";

        public static IServiceCollection AddRedisCaching(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = configuration.GetSection(RedisSection)[RedisConfigurationSection];
                options.InstanceName = configuration.GetSection(RedisSection)[RedisInstanceNameSection];
            });

            services.AddHealthChecks()
                .AddRedis(configuration.GetSection(RedisSection)[RedisConfigurationSection]!);

            return services;
        }
    }
}
