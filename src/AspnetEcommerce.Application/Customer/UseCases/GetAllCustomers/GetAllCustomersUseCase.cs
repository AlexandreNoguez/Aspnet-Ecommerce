
using AspnetEcommerce.Application.Customer.DTOs.GetAllCustomer;
using AspnetEcommerce.Domain.Customer.Repository;

namespace AspnetEcommerce.Application.Customer.UseCases.GetAllCustomers;

public sealed class GetAllCustomersUseCase : IGetAllCustomersUseCase
{
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    private readonly ICustomerRepository _customerRepository;

    public GetAllCustomersUseCase(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository ?? throw new ArgumentNullException(nameof(customerRepository));
    }

    public async Task<GetAllCustomersOutput> ExecuteAsync(
        GetAllCustomersInput input,
        CancellationToken cancellationToken = default)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));

        var (page, pageSize) = NormalizePagination(input.Page, input.PageSize);

        var result = await _customerRepository.GetPagedAsync(page, pageSize, input.Search, cancellationToken);

        var totalPages = CalculateTotalPages(result.TotalItems, pageSize);

        var items = result.Items
            .Select(customer => new GetAllCustomersItemOutput(
                customer.Id,
                customer.Name,
                customer.Email,
                customer.Address?.Street,
                customer.Address?.City,
                customer.Address?.State,
                customer.Address?.ZipCode,
                customer.Address?.Number,
                customer.IsActive,
                customer.RewardPoints
                ))
            .ToList();

        return new GetAllCustomersOutput(
            items,
            page,
            pageSize,
            result.TotalItems,
            totalPages);
    }

    private static (int Page, int PageSize) NormalizePagination(int page, int pageSize)
    {
        var normalizedPage = page < 1 ? DefaultPage : page;
        var normalizedPageSize = pageSize < 1 ? DefaultPageSize : Math.Min(pageSize, MaxPageSize);

        return (normalizedPage, normalizedPageSize);
    }

    private static int CalculateTotalPages(int totalItems, int pageSize)
    {
        if (totalItems == 0) return 0;

        return (int)Math.Ceiling(totalItems / (double)pageSize);
    }
}