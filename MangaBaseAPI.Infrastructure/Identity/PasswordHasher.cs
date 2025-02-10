using MangaBaseAPI.CrossCuttingConcerns.Identity;
using MangaBaseAPI.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace MangaBaseAPI.Infrastructure.Identity
{
    public class PasswordHasher : IPasswordHasher
    {
        readonly PasswordHasher<User> _hasher = new PasswordHasher<User>();

        public string HashProvidedPassword(User user, string providedPassword)
        {
            return _hasher.HashPassword(user, providedPassword);
        }

        public bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword)
        {
            return _hasher.VerifyHashedPassword(user, hashedPassword, providedPassword) == PasswordVerificationResult.Success;
        }
    }
}
