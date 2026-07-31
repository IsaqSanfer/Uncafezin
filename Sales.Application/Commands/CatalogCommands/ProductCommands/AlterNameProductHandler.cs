using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Catalog.ValueObjects;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.CatalogCommands.ProductCommands;

public sealed class AlterNameProductHandler
{
    private readonly IProductRepository _productRepository;

    public AlterNameProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<AlterNameProductResultDTO> HandleAsync(AlterNameProductCommand command, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
            throw new DomainException($"Product with ID {command.ProductId} not found.");

        var newName = new ProductName(command.NewName);

        product.AlterName(newName);

        await _productRepository.UpdateAsync(product, cancellationToken);

        return new AlterNameProductResultDTO
        {
            ProductId = product.Id,
            Name = product.Name.Value
        };
    }
}
