using Hangfire;
using MangaBaseAPI.CrossCuttingConcerns.BackgroundJob.HangfireScheduler;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace MangaBaseAPI.Infrastructure.BackgroundJob.HangfireScheduler
{
    public static class HangfireServiceCollectionExtensions
    {
        const string HangfireConnectionString = "HangfireDB";

        public static IServiceCollection AddHangfireServices(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.AddHangfire(config =>
            {
                config.UseSqlServerStorage(configuration.GetConnectionString(HangfireConnectionString));
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
