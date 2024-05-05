using DDD_CQRS_Sample.Application.Products.GeProductById;
using DDD_CQRS_Sample.Application.Products.GetProductList;
using DDD_CQRS_Sample.Domain.Products;
using Shared.DataGrids;

namespace DDD_CQRS_Sample.Application.Products;

public interface IProductRepository
{
    void Add(Product product);
    void Delete(Product product);
    Task<ProductResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<Product?> FindByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<DataGridResponse<ProductListResponse>> GetList(GetProductListQuery request, CancellationToken cancellationToken = default);
    Task<bool> IsDuplicateNameAndBrand(int id, string name, string brand, CancellationToken cancellationToken = default);
}