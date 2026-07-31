using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.CatalogCommands.ProductCommands;

public sealed class AdjustStockHandler
{
    private readonly IProductRepository _productRepository;

    public AdjustStockHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<AdjustStockResultDTO> HandleAsync(AdjustStockCommand command, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
            throw new DomainException($"Product with ID {command.ProductId} not found.");

        product.AdjustStock(command.Quantity, command.Reason);

        await _productRepository.UpdateAsync(product, cancellationToken);

        return new AdjustStockResultDTO
        {
            ProductId = product.Id,
            Stock = product.Stock
        };
    }
}
