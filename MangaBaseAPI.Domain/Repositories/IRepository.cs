using MangaBaseAPI.Domain.Abstractions.Specification;

namespace MangaBaseAPI.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetQueryableSet();

        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

        Task<T?> SingleOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

        Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default);

        Task BulkInsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);

        void BulkUpdate(IEnumerable<TEntity> entities);

        void BulkDelete(IEnumerable<TEntity> entities);

        IQueryable<TEntity> ApplySpecification(
            Specification<TEntity> specification);
    }
}
