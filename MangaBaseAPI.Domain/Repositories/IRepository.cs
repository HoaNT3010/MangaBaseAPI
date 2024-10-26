namespace MangaBaseAPI.Domain.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        IQueryable<TEntity> GetQueryableSet();

        Task AddAsync(TEntity entity, CancellationToken cancellationToken = default);

        void UpdateAsync(TEntity entity);

        void Delete(TEntity entity);

        Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query);

        Task<T?> SingleOrDefaultAsync<T>(IQueryable<T> query);

        Task<List<T>> ToListAsync<T>(IQueryable<T> query);

        Task BulkInsertAsync(IEnumerable<TEntity> entities);

        void BulkUpdate(IEnumerable<TEntity> entities);

        void BulkDelete(IEnumerable<TEntity> entities);
    }
}
