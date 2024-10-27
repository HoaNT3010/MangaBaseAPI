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

        public TRepository GetRepository<TRepository>() where TRepository : class
        {
            return _serviceProvider.GetRequiredService<TRepository>();
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
