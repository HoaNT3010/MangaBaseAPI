using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Repositories;

namespace MangaBaseAPI.Persistence.Repositories
{
    public class TitleGenreRepository : Repository<TitleGenre>, ITitleGenreRepository
    {
        public TitleGenreRepository(MangaBaseDbContext context) : base(context)
        {
        }
    }
}
