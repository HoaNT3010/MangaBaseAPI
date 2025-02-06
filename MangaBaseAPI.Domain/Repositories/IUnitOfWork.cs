namespace MangaBaseAPI.Domain.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        TRepository GetRepository<TRepository>() where TRepository : class;
        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default);
        Task BeginTransactionAsync(CancellationToken cancellationToken = default);
        Task CommitTransactionAsync(CancellationToken cancellationToken = default);
        Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
    }
}
