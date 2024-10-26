using MangaBaseAPI.Domain.Repositories;

namespace MangaBaseAPI.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MangaBaseDbContext _context;
        private readonly Dictionary<string, object> _repositories;

        public UnitOfWork(MangaBaseDbContext context)
        {
            _context = context;
            _repositories = new Dictionary<string, object>();
        }

        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitTransactionAsync()
        {
            await _context.Database.CommitTransactionAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IRepository<TEntity> Repository<TEntity>() where TEntity : class
        {
            var type = typeof(TEntity).Name;
            if (!_repositories.ContainsKey(type))
            {
                var repositoryInstance = new Repository<TEntity>(_context);
                _repositories.Add(type, repositoryInstance);
            }
            return (IRepository<TEntity>)_repositories[type];
        }

        public async Task RollbackTransactionAsync()
        {
            await _context.Database.RollbackTransactionAsync();
        }

        public async Task<int> SaveChangeAsync()
        {
            return await _context.SaveChangesAsync();
        }
    }
}
