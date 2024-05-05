using DDD_CQRS_Sample.Domain.Products;
using Shared.DbContexts;
using Shared.MediatR.Messaging;
using Shared.Results;

namespace DDD_CQRS_Sample.Application.Products.AddProduct;

internal class AddProductCommandHandler : ICommandHandler<AddProductCommand>
{
    private readonly IProductRepository _productRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AddProductCommandHandler(
        IProductRepository productRepository,
        IUnitOfWork unitOfWork)
    {
        _productRepository = productRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(AddProductCommand request, CancellationToken cancellationToken)
    {
        var isDuplicateName = await _productRepository.IsDuplicateNameAndBrand(0, request.Name, request.Brand, cancellationToken);

        if (isDuplicateName)
        {
            return Result.Failure(ProductErrors.DuplicateNameAndBrand);
        }

        var product = Product.Create(
            request.Name,
            request.Brand,
            request.Description,
            request.Price,
            request.ImageId,
            request.ExtraInfos);

        _productRepository.Add(product);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
