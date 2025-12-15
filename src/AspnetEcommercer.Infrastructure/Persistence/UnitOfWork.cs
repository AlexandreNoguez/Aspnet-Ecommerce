using AspnetEcommercer.Domain.Contracts.Abstractions;
using AspnetEcommercer.Infrastructure.Database;

namespace AspnetEcommercer.Infrastructure.Persistence
{


    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _context;

        public UnitOfWork(DatabaseContext context)
        {
            _context = context;
        }

        public Task CommitAsync() => _context.SaveChangesAsync();
    }
}
