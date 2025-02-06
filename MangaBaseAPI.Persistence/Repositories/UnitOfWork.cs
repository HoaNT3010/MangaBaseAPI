using MangaBaseAPI.Domain.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace MangaBaseAPI.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly MangaBaseDbContext _context;
        private readonly IServiceProvider _serviceProvider;

        public UnitOfWork(
            MangaBaseDbContext context,
            IServiceProvider serviceProvider)
        {
            _context = context;
            _serviceProvider = serviceProvider;
        }

        public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.BeginTransactionAsync(cancellationToken);
        }

        public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.CommitTransactionAsync(cancellationToken);
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public TRepository GetRepository<TRepository>() where TRepository : class
        {
            return _serviceProvider.GetRequiredService<TRepository>();
        }

        public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
        {
            await _context.Database.RollbackTransactionAsync(cancellationToken);
        }

        public async Task<int> SaveChangeAsync(CancellationToken cancellationToken = default)
        {
            return await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
