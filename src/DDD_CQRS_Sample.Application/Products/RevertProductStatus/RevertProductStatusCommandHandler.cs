using DDD_CQRS_Sample.Domain.Products;
using Shared.Data;
using Shared.MediatR.Messaging;
using Shared.Results;

namespace DDD_CQRS_Sample.Application.Products.RevertProductStatus;

internal class RevertProductStatusCommandHandler : ICommandHandler<RevertProductStatusCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public RevertProductStatusCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(RevertProductStatusCommand request, CancellationToken cancellationToken)
    {
        var product = await _productRepository.FindByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            return Result.Failure(ProductErrors.NotFound);
        }

        product.RevertStatus();

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
