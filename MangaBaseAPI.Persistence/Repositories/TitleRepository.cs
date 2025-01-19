using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaBaseAPI.Persistence.Repositories
{
    public class TitleRepository : Repository<Title>, ITitleRepository
    {
        public TitleRepository(MangaBaseDbContext context) : base(context)
        {
        }

        public async Task<bool> IsTitleDeleted(Guid titleId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(x => x.Id == titleId && x.IsDeleted, cancellationToken);
        }

        public async Task<bool> IsTitleExists(Guid titleId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(x => x.Id == titleId, cancellationToken);
        }

        public async Task<bool> IsTitleHidden(Guid titleId, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(x => x.Id == titleId && x.IsHidden, cancellationToken);
        }

        public async Task<bool> IsTitleNameTaken(string titleName, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AnyAsync(x => x.Name == titleName, cancellationToken);
        }
    }
}
