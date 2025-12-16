using AspnetEcommerce.Domain.Contracts.Abstractions;
using AspnetEcommerce.Infrastructure.Database;
using Microsoft.EntityFrameworkCore.Storage;

namespace AspnetEcommerce.Infrastructure.Persistence
{


    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;
        private IDbContextTransaction? _currentTransaction;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public async Task BeginTransactionAsync(CancellationToken ct = default)
        {
            if (_currentTransaction is not null)
            {
                return;
            }

            _currentTransaction = await _context.Database.BeginTransactionAsync(ct);
        }

        public async Task CommitAsync(CancellationToken ct = default)
        {
            await _context.SaveChangesAsync(ct);

            if (_currentTransaction is not null)
            {
                await _currentTransaction.CommitAsync(ct);
                await _currentTransaction.DisposeAsync();
                _currentTransaction = null;
            }
        }

        public async Task RollbackAsync(CancellationToken ct = default)
        {
            if (_currentTransaction is null)
            {
                return;
            }

            await _currentTransaction.RollbackAsync(ct);
            await _currentTransaction.DisposeAsync();
            _currentTransaction = null;
        }
    }
}
