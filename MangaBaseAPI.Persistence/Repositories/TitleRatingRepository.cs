using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Repositories;

namespace MangaBaseAPI.Persistence.Repositories
{
    public class TitleRatingRepository : Repository<TitleRating>, ITitleRatingRepository
    {
        public TitleRatingRepository(MangaBaseDbContext context) : base(context)
        {
        }
    }
}
