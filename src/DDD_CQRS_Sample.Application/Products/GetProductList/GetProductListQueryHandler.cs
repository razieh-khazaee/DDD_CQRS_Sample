using Shared.DataGrids;
using Shared.MediatR.Messaging;
using Shared.Results;

namespace DDD_CQRS_Sample.Application.Products.GetProductList;

internal class GetProductListQueryHandler : IQueryHandler<GetProductListQuery, DataGridResponse<ProductListResponse>>
{
    private readonly IProductRepository _productRepository;

    public GetProductListQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<DataGridResponse<ProductListResponse>>> Handle(GetProductListQuery request, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetList(request);
        return Result<DataGridResponse<ProductListResponse>>.Success(products);
    }
}