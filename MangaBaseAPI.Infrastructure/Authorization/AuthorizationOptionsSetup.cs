using MangaBaseAPI.Domain.Constants.Authorization;
using MangaBaseAPI.Domain.Constants.Role;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace MangaBaseAPI.Infrastructure.Authorization
{
    public class AuthorizationOptionsSetup : IConfigureOptions<AuthorizationOptions>
    {
        public void Configure(AuthorizationOptions options)
        {
            options.AddPolicy(Policies.AdminRole, policy =>
            {
                policy.RequireRole(ApplicationRoles.Administrator);
            });

            options.AddPolicy(Policies.MemberRole, policy =>
            {
                policy.RequireRole(ApplicationRoles.Member);
            });
        }
    }
}
