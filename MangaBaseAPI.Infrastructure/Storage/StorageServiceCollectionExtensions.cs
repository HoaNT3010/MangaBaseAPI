using Google.Cloud.Storage.V1;
using MangaBaseAPI.CrossCuttingConcerns.Storage.GoogleCloudStorage;
using MangaBaseAPI.Domain.Constants.Application;
using MangaBaseAPI.Infrastructure.Storage.GoogleCloudStorage;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace MangaBaseAPI.Infrastructure.Storage
{
    public static class StorageServiceCollectionExtensions
    {
        public static IServiceCollection AddStorageServices(
            this IServiceCollection services,
            string environment = ApplicationConstants.DevelopmentEnvironment)
        {
            services.AddGoogleCloudStorage(environment);

            return services;
        }

        private static IServiceCollection AddGoogleCloudStorage(
            this IServiceCollection services,
            string environment = ApplicationConstants.DevelopmentEnvironment)
        {
            GetProductionGcsKey(environment);
            services.ConfigureOptions<GoogleCloudStorageOptionsSetup>();
            services.AddSingleton(StorageClient.Create());
            services.AddSingleton<IGoogleCloudStorageService, GoogleCloudStorageService>();

            return services;
        }

        private static void GetProductionGcsKey(string environment = ApplicationConstants.DevelopmentEnvironment)
        {
            if (environment != ApplicationConstants.ProductionEnvironment) return;
            var gcsKeyBase64 = Environment.GetEnvironmentVariable("GCS_KEY_BASE64");
            const string gcsKeyPath = "gcs-key.json";
            if (string.IsNullOrEmpty(gcsKeyBase64)) return;
            string jsonKey = Encoding.UTF8.GetString(Convert.FromBase64String(gcsKeyBase64));
            var gcsKeyBytes = Convert.FromBase64String(gcsKeyBase64);
            File.WriteAllText(gcsKeyPath, jsonKey);
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", gcsKeyPath);
        }
    }
}
