using MangaBaseAPI.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace MangaBaseAPI.Persistence.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private readonly MangaBaseDbContext _context;
        private readonly DbSet<TEntity> _dbSet;

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

        public async Task BulkInsertAsync(IEnumerable<TEntity> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void BulkUpdate(IEnumerable<TEntity> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public void Delete(TEntity entity)
        {
            _dbSet.Remove(entity);
        }

        public async Task<T?> FirstOrDefaultAsync<T>(IQueryable<T> query)
        {
            return await query.FirstOrDefaultAsync();
        }

        public IQueryable<TEntity> GetQueryableSet()
        {
            return _dbSet;
        }

        public async Task<T?> SingleOrDefaultAsync<T>(IQueryable<T> query)
        {
            return await query.SingleOrDefaultAsync();
        }

        public async Task<List<T>> ToListAsync<T>(IQueryable<T> query)
        {
            return await query.ToListAsync();
        }

        public void UpdateAsync(TEntity entity)
        {
            _dbSet.Update(entity);
        }
    }
}
