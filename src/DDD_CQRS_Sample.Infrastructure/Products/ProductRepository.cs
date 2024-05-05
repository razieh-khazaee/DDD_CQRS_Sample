using DDD_CQRS_Sample.Application.Products;
using DDD_CQRS_Sample.Application.Products.GeProductById;
using DDD_CQRS_Sample.Application.Products.GetProductList;
using DDD_CQRS_Sample.Domain.Products;
using DDD_CQRS_Sample.Infrastructure.DbContexts;
using Microsoft.EntityFrameworkCore;
using Shared.Audit;
using Shared.DataGrids;

namespace DDD_CQRS_Sample.Infrastructure.Products;

internal class ProductRepository : IProductRepository
{
    private readonly DDD_CQRS_SampleDbContext dbContext;

    public ProductRepository(DDD_CQRS_SampleDbContext dbContext)
    {
        this.dbContext = dbContext;
    }

    public virtual void Add(Product product)
    {
        dbContext.Add(product);
    }

    public virtual void Delete(Product product)
    {
        dbContext.Remove(product);
    }

    public async Task<ProductResponse?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Products.AsNoTracking()
            .Where(m => m.Id == id)
            .Select(m => new ProductResponse(
                     m.Id,
                     m.Name,
                     m.Brand,
                     m.Description,
                     m.Price,
                     m.Inventory,
                     m.ExtraInfos.ToList(),
                     m.IsActive,
                     EF.Property<string>(m, AuditConstants.CreatedBy),
                     EF.Property<DateTime>(m, AuditConstants.CreatedDate),
                     EF.Property<string>(m, AuditConstants.UpdatedBy),
                     EF.Property<DateTime?>(m, AuditConstants.UpdatedDate)))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<Product?> FindByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
              .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
    }

    public Task<DataGridResponse<ProductListResponse>> GetList(GetProductListQuery request, CancellationToken cancellationToken = default)
    {
        var query = dbContext.Products.AsNoTracking();
        if (!string.IsNullOrWhiteSpace(request.Name))
        {
            query = query.Where(m => m.Name.Contains(request.Name));
        }
        if (!string.IsNullOrWhiteSpace(request.Brand))
        {
            query = query.Where(m => m.Brand.Contains(request.Brand));
        }
        if (request.IsActive != null)
        {
            query = query.Where(m => m.IsActive == request.IsActive);
        }
        if (request.Price != null)
        {
            query = query.Where(m => m.Price >= request.Price);
        }

        IQueryable<ProductListResponse> resultQuery = query.OrderBy(m => m.Name).Select(m => new ProductListResponse(
             m.Id,
             m.Name,
             m.Brand,
             m.Price,
             m.Inventory,
             m.IsActive));

        var result = DataGridResponse<ProductListResponse>.Create(resultQuery, request.Page, request.PageSize);
        return result;
    }

    public async Task<bool> IsDuplicateNameAndBrand(int id, string name, string brand, CancellationToken cancellationToken = default)
    {
        return await dbContext.Products
               .AnyAsync(m => m.Id != id &&
                            m.Name == name &&
                            m.Brand == brand,
                            cancellationToken);
    }
}