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

        public async Task<bool> IsTitleNameTaken(string titleName)
        {
            return await _dbSet.AnyAsync(x => x.Name == titleName);
        }
    }
}
