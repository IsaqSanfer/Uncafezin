using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Catalog;
using Sales.Domain.Catalog.ValueObjects;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.CatalogCommands.ProductCommands;

public sealed class CreateProductHandler
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public CreateProductHandler(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }

    public async Task<CreateProductResultDTO> HandleAsync(CreateProductCommand command, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);
        if (category is null)
            throw new DomainException($"Category with ID {command.CategoryId} does not exist.");

        Guard.Against<DomainException>(!category.Active, "Category is not active.");

        var name = new ProductName(command.Name);
        var code = new ProductCode(command.Code);
        var price = new ProductPrice(command.Price);

        var product = new Product(name, code, price, command.CategoryId, command.InitialStock, command.Description);

        await _productRepository.AddAsync(product, cancellationToken);

        return new CreateProductResultDTO
        {
            ProductId = product.Id,
            Name = product.Name.Value,
            Price = product.Price.Value,
            Status = product.Status.ToString()
        };
    }
}
