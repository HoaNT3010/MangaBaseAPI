using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Repositories;

namespace MangaBaseAPI.Persistence.Repositories
{
    public class TitleAuthorRepository : Repository<TitleAuthor>, ITitleAuthorRepository
    {
        public TitleAuthorRepository(MangaBaseDbContext context) : base(context)
        {
        }
    }
}
