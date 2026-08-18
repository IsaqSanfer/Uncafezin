using Sales.Domain.Orders.Integration.Catalog;

namespace Sales.Infrastructure.Gateways;

public sealed class CatalogGateway : ICatalogGateway
{
    private static readonly Dictionary<Guid, ProductDTO> _products = new()
    {
        [new Guid("11111111-1111-1111-1111-111111111111")] = new ProductDTO(new Guid("11111111-1111-1111-1111-111111111111"), "Product 1", 10.00m),
        [new Guid("22222222-2222-2222-2222-222222222222")] = new ProductDTO(new Guid("22222222-2222-2222-2222-222222222222"), "Product 2", 20.00m),
        [new Guid("33333333-3333-3333-3333-333333333333")] = new ProductDTO(new Guid("33333333-3333-3333-3333-333333333333"), "Product 3", 30.00m),
        [new Guid("44444444-4444-4444-4444-444444444444")] = new ProductDTO(new Guid("44444444-4444-4444-4444-444444444444"), "Product 4", 40.00m),
        [new Guid("55555555-5555-5555-5555-555555555555")] = new ProductDTO(new Guid("55555555-5555-5555-5555-555555555555"), "Product 5", 50.00m),
        [new Guid("66666666-6666-6666-6666-666666666666")] = new ProductDTO(new Guid("66666666-6666-6666-6666-666666666666"), "Product 6", 60.00m)
    };

    public Task<ProductDTO?> GetProductByIdAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        _products.TryGetValue(productId, out var product);
        return Task.FromResult(product);
    }
}
