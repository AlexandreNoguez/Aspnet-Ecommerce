namespace AspnetEcommercer.Domain.Contracts.Abstractions
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
