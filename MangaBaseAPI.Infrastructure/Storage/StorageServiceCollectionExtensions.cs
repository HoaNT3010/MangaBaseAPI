using Google.Cloud.Storage.V1;
using MangaBaseAPI.CrossCuttingConcerns.Storage.GoogleCloudStorage;
using MangaBaseAPI.Infrastructure.Storage.GoogleCloudStorage;
using Microsoft.Extensions.DependencyInjection;

namespace MangaBaseAPI.Infrastructure.Storage
{
    public static class StorageServiceCollectionExtensions
    {
        public static IServiceCollection AddStorageServices(this IServiceCollection services) 
        {
            services.AddGoogleCloudStorage();

            return services;
        }

        private static IServiceCollection AddGoogleCloudStorage(this IServiceCollection services)
        {
            services.ConfigureOptions<GoogleCloudStorageOptionsSetup>();
            services.AddSingleton(StorageClient.Create());
            services.AddSingleton<IGoogleCloudStorageService, GoogleCloudStorageService>();

            return services;
        }
    }
}
