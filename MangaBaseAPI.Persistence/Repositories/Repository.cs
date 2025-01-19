using MangaBaseAPI.Domain.Abstractions.Specification;
using MangaBaseAPI.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaBaseAPI.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly MangaBaseDbContext _context;
        protected readonly DbSet<TEntity> _dbSet;

        public Repository(MangaBaseDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TEntity>();
        }

        public async Task AddAsync(TEntity entity, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddAsync(entity, cancellationToken);
        }

        public void BulkDelete(IEnumerable<TEntity> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task BulkInsertAsync(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public void BulkUpdate(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
        {
            return await query.FirstOrDefaultAsync(cancellationToken);
        }

        public IQueryable<TEntity> GetQueryableSet()
        {
            return _dbSet;
        }

        public async Task<T?> SingleOrDefaultAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
        {
            return await query.SingleOrDefaultAsync(cancellationToken);
        }

        public async Task<List<T>> ToListAsync<T>(IQueryable<T> query, CancellationToken cancellationToken = default)
        {
            return await query.ToListAsync(cancellationToken);
        }

        public void Update(TEntity entity)
        {
            _dbSet.Update(entity);
        }

        public IQueryable<TEntity> ApplySpecification(Specification<TEntity> specification)
        {
            return SpecificationEvaluator.GetQuery(
                GetQueryableSet(),
                specification);
        }
    }
}
