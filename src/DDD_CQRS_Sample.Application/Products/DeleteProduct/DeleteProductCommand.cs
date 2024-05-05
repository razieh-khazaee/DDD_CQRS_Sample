using Shared.MediatR.Messaging;

namespace DDD_CQRS_Sample.Application.Products.DeleteProduct;

public record DeleteProductCommand(int Id) : ICommand;
