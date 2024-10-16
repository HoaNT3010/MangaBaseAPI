using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.CrossCuttingConcerns.Identity
{
    public interface IPasswordHasher
    {
        bool VerifyHashedPassword(User user, string hashedPassword, string providedPassword);
        string HashProvidedPassword(User user, string providedPassword);
    }
}
