namespace DDD_CQRS_Sample.Application.Products.GetProductList;

public record ProductListResponse(
    int Id,
    string Name,
    string Brand,
    decimal Price,
    int Inventory,
    bool IsActive);