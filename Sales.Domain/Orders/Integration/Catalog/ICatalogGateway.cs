namespace Sales.Domain.Orders.Integration.Catalog;

public interface ICatalogGateway
{
    Task<ProductDTO?> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken = default);
}
