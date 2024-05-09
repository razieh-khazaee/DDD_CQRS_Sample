using DDD_CQRS_Sample.Application.Products.Shared;

namespace DDD_CQRS_Sample.Application.Products.GeProductById;

public record ProductResponse(
    int Id,
    string Name,
    string Brand,
    string? Description,
    decimal Price,
    int Inventory,
    List<ExtraInfoDto> extraInfos,
    bool IsActive,
    string CreatedBy,
    DateTime CreatedDate,
    string? UpdatedBy,
    DateTime? UpdatedDate);