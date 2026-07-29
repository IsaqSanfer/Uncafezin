using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Application.Commands.CatalogCommands.ProductCommands;

public sealed class AlterCategoryProductHandler
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public AlterCategoryProductHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<AlterCategoryProductResultDTO> HandleAsync(AlterCategoryProductCommand command, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(command.NewCategoryId, cancellationToken);
        if (category == null)
            throw new DomainException($"Category with ID {command.NewCategoryId} not found.");

        Guard.Against<DomainException>(!category.Active, "New category is inactive. Cannot assign product to an inactive category.");

        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);
        if (product == null)
            throw new DomainException($"Product with ID {command.ProductId} not found.");

        product.AlterCategory(command.NewCategoryId);

        await _productRepository.UpdateAsync(product, cancellationToken);

        return new AlterCategoryProductResultDTO
        {
            ProductId = product.Id,
            CategoryId = product.CategoryId
        };
    }
}
