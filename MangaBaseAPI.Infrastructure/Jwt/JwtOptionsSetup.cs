using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace MangaBaseAPI.Infrastructure.Jwt
{
    public class JwtOptionsSetup : IConfigureOptions<JwtOptions>
    {
        const string SectionName = "Jwt";
        readonly IConfiguration _configuration;

        public JwtOptionsSetup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void Configure(JwtOptions options)
        {
            _configuration.GetSection(SectionName).Bind(options);
        }
    }
}
