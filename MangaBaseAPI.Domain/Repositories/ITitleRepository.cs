using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Domain.Repositories
{
    public interface ITitleRepository : IRepository<Title>
    {
        Task<bool> IsTitleNameTaken(string titleName);
    }
}
