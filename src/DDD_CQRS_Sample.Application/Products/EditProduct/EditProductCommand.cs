using DDD_CQRS_Sample.Domain.Products;
using Shared.MediatR.Messaging;

namespace DDD_CQRS_Sample.Application.Products.EditProduct;

public record EditProductCommand(
    int Id,
    string Name,
    string Brand,
    string Description,
    decimal Price,
    int? ImageId,
    List<ExtraInfoValueObject> ExtraInfos) : ICommand;