using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.CatalogCommands.ProductCommands;

public sealed class ActivateProductHandler
{
    private readonly IProductRepository _productRepository;

    public ActivateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<ActivateProductResultDTO> HandleAsync(ActivateProductCommand command, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
            throw new DomainException($"Product with ID {command.ProductId} not found.");

        product.Activate();

        await _productRepository.UpdateAsync(product, cancellationToken);

        return new ActivateProductResultDTO
        {
            ProductId = product.Id,
            Status = product.Status.ToString()
        };
    }
}
