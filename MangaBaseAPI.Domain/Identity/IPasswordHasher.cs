using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Domain.Identity
{
    public interface IPasswordHasher
    {
        bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword);
        string HashProvidedPassword(User user, string providedPassword);
    }
}
