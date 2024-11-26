using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Repositories;

namespace MangaBaseAPI.Persistence.Repositories
{
    public class AlternativeNameRepository : Repository<AlternativeName>, IAlternativeNameRepository
    {
        public AlternativeNameRepository(MangaBaseDbContext context) : base(context)
        {
        }
    }
}
