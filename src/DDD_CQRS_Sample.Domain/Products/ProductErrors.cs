using Shared.Results;

namespace DDD_CQRS_Sample.Domain.Products;

public static class ProductErrors
{
    public static readonly Error DuplicateNameAndBrand = new(
        "Product.DuplicateName",
        "نام و برند محصول تکراری است");

    public static readonly Error NotFound = new(
        "Product.NotFound",
        "محصول مورد نظر وجود ندارد");

    public static readonly Error NotEnoughInventory = new Error("Product.NotEnoughInventory", "موجودی کالا کافی نمی باشد");
}