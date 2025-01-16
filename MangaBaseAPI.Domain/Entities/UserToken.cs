using Microsoft.AspNetCore.Identity;

namespace MangaBaseAPI.Domain.Entities
{
    public class UserToken : IdentityUserToken<Guid>
    {
        public DateTimeOffset? TokenExpiry { get; set; }
    }
}
