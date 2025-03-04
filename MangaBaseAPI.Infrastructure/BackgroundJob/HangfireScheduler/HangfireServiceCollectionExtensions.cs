using Hangfire;
using MangaBaseAPI.CrossCuttingConcerns.BackgroundJob.HangfireScheduler;
using MangaBaseAPI.Domain.Constants.Application;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MangaBaseAPI.Infrastructure.BackgroundJob.HangfireScheduler
{
    public static class HangfireServiceCollectionExtensions
    {
        const string DefaultHangfireConnectionString = "HangfireDB";
        const string ProductionHangfireConnectionString = "HangfireDB_Production";

        public static IServiceCollection AddHangfireServices(
            this IServiceCollection services,
            IConfiguration configuration,
            string environment = ApplicationConstants.DevelopmentEnvironment)
        {
            services.AddHangfire(config =>
            {
                switch (environment)
                {
                    case ApplicationConstants.DevelopmentEnvironment:
                        config.UseSqlServerStorage(configuration.GetConnectionString(DefaultHangfireConnectionString));
                        break;
                    case ApplicationConstants.ProductionEnvironment:
                        config.UseSqlServerStorage(configuration.GetConnectionString(ProductionHangfireConnectionString));
                        break;
                    default:
                        config.UseSqlServerStorage(configuration.GetConnectionString(DefaultHangfireConnectionString));
                        break;
                }
                config.UseSimpleAssemblyNameTypeSerializer();
                config.UseRecommendedSerializerSettings();
            });

            services.AddHangfireServer(opt =>
            {
                opt.SchedulePollingInterval = TimeSpan.FromSeconds(30);
            });

            services.AddScoped<IHangfireBackgroundJobService, HangfireBackgroundJobService>();

            services.AddHangfireHealthCheck();

            return services;
        }

        public static WebApplication AddHangfireDashboard(this WebApplication application)
        {
            if (application.Environment.IsDevelopment())
            {
                application.UseHangfireDashboard();
            }

            return application;
        }

        private static IServiceCollection AddHangfireHealthCheck(this IServiceCollection services)
        {
            services.AddHealthChecks()
                .AddHangfire(setup => { });

            return services;
        }
    }
}
