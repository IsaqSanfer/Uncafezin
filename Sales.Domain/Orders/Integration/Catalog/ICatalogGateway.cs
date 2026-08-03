namespace Sales.Domain.Orders.Integration.Catalog;

public interface ICatalogGateway
{
    Task<ProductDTO?> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken = default);
    Task<bool> GetAvailableStockAsync(Guid productId, int quantity, CancellationToken cancellationToken = default);
}
