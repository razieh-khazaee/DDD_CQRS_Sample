using DDD_CQRS_Sample.Application.Products.GeProductById;
using DDD_CQRS_Sample.Domain.Products.Events;
using MediatR;
using Shared.Email;

namespace DDD_CQRS_Sample.Application.Products.DecreaseInventory;

internal sealed class ProductInventoryDecreasedDomainEventHandler : INotificationHandler<ProductInventoryDecreasedDomainEvent>
{
    private readonly IProductRepository productRepository;
    private readonly IEmailService emailService;

    public ProductInventoryDecreasedDomainEventHandler(IProductRepository productRepository,
        IEmailService emailService)
    {
        this.productRepository = productRepository;
        this.emailService = emailService;
    }

    public async Task Handle(ProductInventoryDecreasedDomainEvent notification, CancellationToken cancellationToken)
    {
        ProductResponse? product = await productRepository.GetByIdAsync(notification.ProductId, cancellationToken);

        if (product is null)
        {
            return;
        }

        await emailService.SendEmail(
            "razieh6522@yahoo.com",
            "Product stock is running low!",
            $"Only {product.Inventory} of product {product.Name}-{product.Brand} remains");
    }
}
