using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Domain.Repositories
{
    public interface IUserTokenRepository : IRepository<UserToken>
    {
        Task<Guid> GetUserIdByTokenValue(string tokenValue);
        Task<bool> TryRemoveTokenByValueAsync(string tokenValue);
    }
}
