using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Domain.Repositories
{
    public interface IUserTokenRepository : IRepository<UserToken>
    {
        Task<Guid> GetUserIdByTokenValue(string tokenValue, CancellationToken cancellationToken = default);
        Task<bool> TryRemoveTokenByValueAsync(string tokenValue, CancellationToken cancellationToken = default);
    }
}
