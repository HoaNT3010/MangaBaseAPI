using MangaBaseAPI.Domain.Constants.Application;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace MangaBaseAPI.Infrastructure.Caching.Redis
{
    public static class RedisServiceCollectionExtensions
    {
        const string RedisSection = "Redis";
        const string RedisInstanceNameSection = "InstanceName";
        const string RedisConfigurationSection = "Configuration";

        public static IServiceCollection AddRedisCaching(
            this IServiceCollection services,
            IConfiguration configuration,
            string environment = ApplicationConstants.DevelopmentEnvironment)
        {
            services.AddStackExchangeRedisCache(options =>
            {
                switch (environment)
                {
                    case ApplicationConstants.DevelopmentEnvironment:
                        options.Configuration = configuration.GetSection(RedisSection)[RedisConfigurationSection];
                        break;
                    case ApplicationConstants.ProductionEnvironment:
                        options.ConfigurationOptions = GetRedisConfigurationOptions(configuration);
                        break;
                    default:
                        options.Configuration = configuration.GetSection(RedisSection)[RedisConfigurationSection];
                        break;
                }
                options.InstanceName = configuration.GetSection(RedisSection)[RedisInstanceNameSection];
            });

            // Configure redis health checks
            switch (environment)
            {
                case ApplicationConstants.DevelopmentEnvironment:
                    services.AddHealthChecks().AddRedis(configuration.GetSection(RedisSection)[RedisConfigurationSection]!);
                    break;
                case ApplicationConstants.ProductionEnvironment:
                    services.AddHealthChecks().AddRedis(ConnectionMultiplexer.Connect(GetRedisConfigurationOptions(configuration)));
                    break;
                default:
                    services.AddHealthChecks().AddRedis(configuration.GetSection(RedisSection)[RedisConfigurationSection]!);
                    break;
            }
            return services;
        }

        const string RedisCloudUser = "RedisCloud:User";
        const string RedisCloudPassword = "RedisCloud:Password";
        const string RedisCloudEndPoint = "RedisCloud:Endpoint";

        private static ConfigurationOptions GetRedisConfigurationOptions(IConfiguration configuration)
        {
            return new ConfigurationOptions()
            {
                User = configuration.GetSection(RedisSection)[RedisCloudUser],
                Password = configuration.GetSection(RedisSection)[RedisCloudPassword],
                EndPoints = { configuration.GetSection(RedisSection)[RedisCloudEndPoint]! },
                Ssl = false,
                AbortOnConnectFail = false
            };
        }
    }
}
