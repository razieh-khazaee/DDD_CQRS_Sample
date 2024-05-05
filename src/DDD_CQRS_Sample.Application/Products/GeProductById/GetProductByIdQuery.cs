using Shared.MediatR.Messaging;

namespace DDD_CQRS_Sample.Application.Products.GeProductById;

public record GetProductByIdQuery(int Id) : IQuery<ProductResponse>;