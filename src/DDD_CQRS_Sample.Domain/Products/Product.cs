using DDD_CQRS_Sample.Domain.Products.Events;
using Shared.Entities;
using Shared.Results;

namespace DDD_CQRS_Sample.Domain.Products;

public class Product : BaseEntity, IAuditable
{
    public string Name { get; private set; }

    public string Brand { get; private set; }

    public string? Description { get; private set; }

    public decimal Price { get; private set; }

    public int Inventory { get; private set; }

    public int? ImageId { get; private set; }


    private List<ExtraInfoValueObject> _extraInfos = new();

    public IReadOnlyCollection<ExtraInfoValueObject> ExtraInfos => _extraInfos.AsReadOnly();

    public bool IsActive { get; private set; }

    private Product()
    {

    }

    private Product(string name,
        string brand,
        string? description,
        decimal price,
        int? imageId,
        List<ExtraInfoValueObject> extraInfos)
    {
        Name = name;
        Brand = brand;
        Description = description;
        Price = price;
        ImageId = imageId;
        _extraInfos = extraInfos;
        IsActive = true;
    }

    public static Product Create(string name,
        string brand,
        string? description,
        decimal price,
        int? imageId,
        List<ExtraInfoValueObject> extraInfos)
    {
        var product = new Product(name, brand, description, price, imageId, extraInfos);
        return product;
    }

    public void Edit(string name,
        string brand,
        string? description,
        decimal price,
        int? imageId,
        List<ExtraInfoValueObject> extraInfos)
    {
        Name = name;
        Description = description;
        Brand = brand;
        Price = price;
        ImageId = imageId;
        _extraInfos = extraInfos;
    }

    public void RevertStatus()
    {
        IsActive = !IsActive;
    }

    public Result DecreaseInventory(int value)
    {
        var newInventory = Inventory - value;
        if (newInventory < 0)
        {
            return Result.Failure(ProductErrors.NotEnoughInventory);
        }

        Inventory = newInventory;

        if (newInventory < 10)
        {
            RaiseDomainEvent(new ProductInventoryDecreasedDomainEvent(Id));
        }

        return Result.Success();
    }
}