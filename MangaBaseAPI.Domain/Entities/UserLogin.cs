using Microsoft.AspNetCore.Identity;

namespace MangaBaseAPI.Domain.Entities
{
    public class UserLogin : IdentityUserLogin<Guid>
    {
        public DateTimeOffset? LoginExpiry { get; set; }
    }
}
