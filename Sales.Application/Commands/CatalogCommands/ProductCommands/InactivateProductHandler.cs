using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.CatalogCommands.ProductCommands;

public sealed class InactivateProductHandler
{
    private readonly IProductRepository _productRepository;

    public InactivateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<InactivateProductResultDTO> HandleAsync(InactivateProductCommand command, CancellationToken cancellationToken = default)
    {
        var product = await _productRepository.GetByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
            throw new DomainException($"Product with ID {command.ProductId} not found.");

        product.Inactivate();

        await _productRepository.UpdateAsync(product, cancellationToken);

        return new InactivateProductResultDTO
        {
            ProductId = product.Id,
            Status = product.Status.ToString()
        };
    }
}
