using DDD_CQRS_Sample.Domain.Products;

namespace DDD_CQRS_Sample.Application.Products.GeProductById;

public record ProductResponse(
    int Id,
    string Name,
    string Brand,
    string? Description,
    decimal Price,
    int Inventory,
    List<ExtraInfoValueObject> extraInfos,
    bool IsActive,
    string CreatedBy,
    DateTime CreatedDate,
    string? UpdatedBy,
    DateTime? UpdatedDate);