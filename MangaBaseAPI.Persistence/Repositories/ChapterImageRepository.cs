using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Repositories;

namespace MangaBaseAPI.Persistence.Repositories
{
    public class ChapterImageRepository : Repository<ChapterImage>, IChapterImageRepository
    {
        public ChapterImageRepository(MangaBaseDbContext context) : base(context)
        {
        }
    }
}
