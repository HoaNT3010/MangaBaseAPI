using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Domain.Repositories
{
    public interface ICreatorRepository : IRepository<Creator>
    {
        Task<bool> IsCreatorNameExist(string creatorName);
    }
}
