using Shared.MediatR.Messaging;

namespace DDD_CQRS_Sample.Application.Products.RevertProductStatus;

public record RevertProductStatusCommand(int Id) : ICommand;
