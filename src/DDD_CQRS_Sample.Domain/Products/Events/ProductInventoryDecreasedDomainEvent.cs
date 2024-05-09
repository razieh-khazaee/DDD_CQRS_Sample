using Shared.Entities;

namespace DDD_CQRS_Sample.Domain.Products.Events
{
    public record ProductInventoryDecreasedDomainEvent(int ProductId) : IDomainEvent;
}
