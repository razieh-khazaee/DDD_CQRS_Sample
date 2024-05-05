using DDD_CQRS_Sample.Domain.Products;
using Shared.MediatR.Messaging;

namespace DDD_CQRS_Sample.Application.Products.AddProduct;

public record AddProductCommand(
    string Name,
    string Brand,
    string Description,
    decimal Price,
    int? ImageId,
    List<ExtraInfoValueObject> ExtraInfos) : ICommand;