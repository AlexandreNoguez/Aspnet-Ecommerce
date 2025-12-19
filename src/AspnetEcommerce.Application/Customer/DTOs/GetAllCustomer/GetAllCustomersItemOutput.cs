namespace AspnetEcommerce.Application.Customer.DTOs.GetAllCustomer;

// Representa UM cliente na lista
public record GetAllCustomersItemOutput(
    Guid Id,
    string Name,
    string Email,
    string? Street,
    string? City,
    string? State,
    string? ZipCode,
    int? Number,
    bool IsActive,
    int RewardPoints);

// Representa a RESPOSTA COMPLETA com paginação
public record GetAllCustomersOutput(
    List<GetAllCustomersItemOutput> Items,
    int Page,
    int PageSize,
    int TotalItems,
    int TotalPages
);