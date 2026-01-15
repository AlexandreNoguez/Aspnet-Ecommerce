using AspnetEcommerce.Application.Product.DTOs.GetAllProducts;
using AspnetEcommerce.Domain.Product.Repository;

namespace AspnetEcommerce.Application.Product.UseCases.GetAllProducts;

public sealed class GetAllProductsUseCase : IGetAllProductsUseCase
{
    private const int DefaultPage = 1;
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 100;

    private readonly IProductRepository _productRepository;

    public GetAllProductsUseCase(IProductRepository productRepository)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
    }

    public async Task<GetAllProductsOutput> ExecuteAsync(
        GetAllProductsInput input,
        CancellationToken cancellationToken = default)
    {
        if (input is null) throw new ArgumentNullException(nameof(input));

        var (page, pageSize) = NormalizePagination(input.Page, input.PageSize);

        var result = await _productRepository.GetPagedAsync(
            page,
            pageSize,
            input.Search,
            input.CategoryId,
            cancellationToken);

        var totalPages = CalculateTotalPages(result.TotalItems, pageSize);

        var items = result.Items
            .Select(product => new GetAllProductsItemOutput(
                product.Id,
                product.Name.Value,
                product.Description.Value,
                product.Price.Amount,
                product.Price.Currency,
                product.Sku.Value,
                product.Category.Id,
                product.Category.Name,
                product.StockQuantity,
                product.IsActive))
            .ToList();

        return new GetAllProductsOutput(
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
