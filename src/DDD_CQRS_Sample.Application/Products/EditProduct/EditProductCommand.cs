using DDD_CQRS_Sample.Application.Products.Shared;
using Shared.MediatR.Messaging;

namespace DDD_CQRS_Sample.Application.Products.EditProduct;

public record EditProductCommand(
    int Id,
    string Name,
    string Brand,
    string Description,
    decimal Price,
    int? ImageId,
    List<ExtraInfoDto> ExtraInfos) : ICommand;