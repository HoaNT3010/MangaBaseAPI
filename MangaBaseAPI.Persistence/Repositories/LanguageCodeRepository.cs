using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Repositories;

namespace MangaBaseAPI.Persistence.Repositories
{
    public class LanguageCodeRepository : Repository<LanguageCode>, ILanguageCodeRepository
    {
        public LanguageCodeRepository(MangaBaseDbContext context) : base(context)
        {
        }
    }
}
