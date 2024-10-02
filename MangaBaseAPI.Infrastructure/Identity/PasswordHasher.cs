using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Identity;
using Microsoft.AspNetCore.Identity;

namespace MangaBaseAPI.Infrastructure.Identity
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashProvidedPassword(User user, string providedPassword)
        {
            return new PasswordHasher<User>().HashPassword(user, providedPassword);
        }

        public bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            return new PasswordHasher<User>().VerifyHashedPassword(user, hashedPassword, providedPassword) == PasswordVerificationResult.Success;
        }
    }
}
