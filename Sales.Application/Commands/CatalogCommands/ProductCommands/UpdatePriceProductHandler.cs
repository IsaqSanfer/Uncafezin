using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Catalog.ValueObjects;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.CatalogCommands.ProductCommands;

public sealed class UpdatePriceProductHandler
{
    private readonly IProductRepository _productRepository;

    public UpdatePriceProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<UpdatePriceProductResultDTO> HandleAsync(UpdatePriceProductCommand command, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
            throw new DomainException($"Product with ID {command.ProductId} not found.");

        var newPrice = new ProductPrice(command.NewPrice);

        product.AlterPrice(newPrice);

        await _productRepository.UpdateAsync(product, cancellationToken);

        return new UpdatePriceProductResultDTO
        {
            ProductId = product.Id,
            Price = product.Price.Value
        };
    }
}
