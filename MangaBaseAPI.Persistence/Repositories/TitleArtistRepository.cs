using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Repositories;

namespace MangaBaseAPI.Persistence.Repositories
{
    public class TitleArtistRepository : Repository<TitleArtist>, ITitleArtistRepository
    {
        public TitleArtistRepository(MangaBaseDbContext context) : base(context)
        {
        }
    }
}
