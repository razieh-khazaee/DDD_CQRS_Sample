using DDD_CQRS_Sample.Domain.Products;
using Shared.Data;
using Shared.MediatR.Messaging;
using Shared.Results;

namespace DDD_CQRS_Sample.Application.Products.EditProduct;

internal class EditProductCommandHandler : ICommandHandler<EditProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EditProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(EditProductCommand request, CancellationToken cancellationToken)
    {
        var isDuplicateName = await _productRepository.IsDuplicateNameAndBrand(request.Id, request.Name, request.Brand, cancellationToken);
        if (isDuplicateName)
        {
            return Result.Failure(ProductErrors.DuplicateNameAndBrand);
        }

        var product = await _productRepository.FindByIdAsync(request.Id, cancellationToken);

        if (product is null)
        {
            return Result.Failure(ProductErrors.NotFound);
        }

        product.Edit(request.Name,
            request.Brand,
            request.Description,
            request.Price,
            request.ImageId,
            request.ExtraInfos.Select(m => new ExtraInfoValueObject(m.Key, m.Value)).ToList());

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}