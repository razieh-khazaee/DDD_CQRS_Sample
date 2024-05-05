using DDD_CQRS_Sample.Domain.Products;
using Shared.MediatR.Messaging;
using Shared.Results;

namespace DDD_CQRS_Sample.Application.Products.GeProductById;

internal class GetProductByIdQueryHandler : IQueryHandler<GetProductByIdQuery, ProductResponse>
{
    private readonly IProductRepository _productRepository;

    public GetProductByIdQueryHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<Result<ProductResponse>> Handle(GetProductByIdQuery request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.GetByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            return Result<ProductResponse>.Failure<ProductResponse>(ProductErrors.NotFound);
        }

        return Result<ProductResponse>.Success(product);
    }
}