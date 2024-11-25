using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MangaBaseAPI.Infrastructure.Storage.GoogleCloudStorage
{
    public class GoogleCloudStorageOptionsSetup : IConfigureOptions<GoogleCloudStorageOptions>
    {
        const string SectionName = "GoogleCloudStorage";
        readonly IConfiguration _configuration;

        public GoogleCloudStorageOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(GoogleCloudStorageOptions options)
        {
            _configuration.GetSection(SectionName).Bind(options);
        }
    }
}
