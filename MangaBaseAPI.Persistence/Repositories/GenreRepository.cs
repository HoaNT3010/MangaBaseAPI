using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Repositories;

namespace MangaBaseAPI.Persistence.Repositories
{
    public class GenreRepository : Repository<Genre>, IGenreRepository
    {
        public GenreRepository(MangaBaseDbContext context) : base(context)
        {
        }
    }
}
