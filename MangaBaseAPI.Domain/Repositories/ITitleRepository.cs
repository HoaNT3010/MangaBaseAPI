using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Domain.Repositories
{
    public interface ITitleRepository : IRepository<Title>
    {
        Task<bool> IsTitleNameTaken(string titleName);
        Task<bool> IsTitleExists(Guid titleId);
        Task<bool> IsTitleDeleted(Guid titleId);
        Task<bool> IsTitleHidden(Guid titleId);
    }
}
