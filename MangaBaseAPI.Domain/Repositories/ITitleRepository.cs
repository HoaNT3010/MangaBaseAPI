using MangaBaseAPI.Domain.Entities;

namespace MangaBaseAPI.Domain.Repositories
{
    public interface ITitleRepository : IRepository<Title>
    {
        Task<bool> IsTitleNameTaken(string titleName, CancellationToken cancellationToken = default);
        Task<bool> IsTitleExists(Guid titleId, CancellationToken cancellationToken = default);
        Task<bool> IsTitleDeleted(Guid titleId, CancellationToken cancellationToken = default);
        Task<bool> IsTitleHidden(Guid titleId, CancellationToken cancellationToken = default);
    }
}
