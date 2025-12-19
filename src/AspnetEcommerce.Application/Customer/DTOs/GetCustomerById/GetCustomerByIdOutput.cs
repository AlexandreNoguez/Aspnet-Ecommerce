namespace AspnetEcommerce.Application.Customer.DTOs.GetCustomerById
{

    public sealed record GetCustomerByIdOutput(
        Guid Id,
        string Name,
        string Email,
        bool IsActive,
        int RewardPoints,
        string? Street,
        string? City,
        string? State,
        string? ZipCode,
        int? Number
    );
}
