using MangaBaseAPI.Domain.Entities;
using MangaBaseAPI.Domain.Repositories;

namespace MangaBaseAPI.Persistence.Repositories
{
    public class CreatorRepository : Repository<Creator>, ICreatorRepository
    {
        public CreatorRepository(MangaBaseDbContext context) : base(context)
        {
        }
    }
}
