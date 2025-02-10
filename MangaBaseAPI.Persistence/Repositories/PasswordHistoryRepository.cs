using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Repositories;

namespace MangaBaseAPI.Persistence.Repositories
{
    public class PasswordHistoryRepository : Repository<PasswordHistory>, IPasswordHistoryRepository
    {
        public PasswordHistoryRepository(MangaBaseDbContext context) : base(context)
        {
        }
    }
}
