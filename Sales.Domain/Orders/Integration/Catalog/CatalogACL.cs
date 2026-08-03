using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Orders.Integration.Catalog;

public sealed class CatalogACL
{
    private readonly ICatalogGateway _catalogGateway;

    public CatalogACL(ICatalogGateway catalogGateway)
    {
        Guard.AgainstNull(catalogGateway, nameof(catalogGateway));
        _catalogGateway = catalogGateway;
    }

    public async Task<ProductSnapshot> GetProductSnapshotAsync(Guid productId, CancellationToken cancellationToken)
    {
        var productDto = await _catalogGateway.GetProductByIdAsync(productId, cancellationToken);

        if (productDto is null)
            throw new DomainException("Product not found in catalog.");

        return new ProductSnapshot(productDto.Id, productDto.Name, productDto.Price);
    }

    public async Task ValidateStockAsync(Guid productId, int quantity, CancellationToken cancellationToken)
    {
        var stockAvailable = await _catalogGateway.GetAvailableStockAsync(productId, quantity, cancellationToken);

        if (!stockAvailable)
            throw new DomainException("Insufficient stock for the product.");
    }
}
