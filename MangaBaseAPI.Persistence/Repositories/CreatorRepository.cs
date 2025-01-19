using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaBaseAPI.Persistence.Repositories
{
    public class CreatorRepository : Repository<Creator>, ICreatorRepository
    {
        public CreatorRepository(MangaBaseDbContext context) : base(context)
        {
        }

        public async Task<bool> IsCreatorNameExist(string creatorName, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(x => x.Name == creatorName, cancellationToken);
        }
    }
}
