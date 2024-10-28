using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaBaseAPI.Persistence.Repositories
{
    public class UserTokenRepository : Repository<UserToken>, IUserTokenRepository
    {
        public UserTokenRepository(MangaBaseDbContext context) : base(context)
        {
        }

        public async Task<Guid> GetUserIdByTokenValue(string tokenValue)
        {
            return await _dbSet.Where(x => x.Value == tokenValue)
                .Select(x => x.UserId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> TryRemoveTokenByValueAsync(string tokenValue)
        {
            var userToken = await FirstOrDefaultAsync(_dbSet.Where(x => x.Value == tokenValue));
            if (userToken == default)
            {
                return false;
            }
            _dbSet.Remove(userToken!);
            return true;
        }
    }
}
