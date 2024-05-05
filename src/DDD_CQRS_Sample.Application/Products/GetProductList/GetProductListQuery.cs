using Shared.DataGrids;
using Shared.MediatR.Messaging;

namespace DDD_CQRS_Sample.Application.Products.GetProductList;

public class GetProductListQuery : DataGridRequest, IQuery<DataGridResponse<ProductListResponse>>
{
    public string? Name { get; set; }

    public string? Brand { get; set; }

    public bool? IsActive { get; set; }

    public decimal? Price { get; set; }
}