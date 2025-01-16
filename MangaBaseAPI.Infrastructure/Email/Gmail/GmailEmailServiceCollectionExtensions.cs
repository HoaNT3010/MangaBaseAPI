using MangaBaseAPI.CrossCuttingConcerns.Email.Gmail;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MangaBaseAPI.Infrastructure.Email.Gmail
{
    public static class GmailEmailServiceCollectionExtensions
    {
        const string GmailEmailSection = "GmailEmail";

        public static IServiceCollection AddGmailEmailService(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<GmailEmailOptions>(configuration.GetSection(GmailEmailSection));
            services.AddTransient<IGmailEmailService, GmailEmailService>();

            return services;
        }
    }
}
