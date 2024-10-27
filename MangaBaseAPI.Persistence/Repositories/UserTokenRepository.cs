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
    }
}
