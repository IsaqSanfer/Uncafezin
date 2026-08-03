namespace Sales.Domain.Orders.Integration.Catalog;

public sealed class CatalogACL
{
    public (string Name, decimal Price) TranslateProduct(ProductDTO product)
    {
        return (product.Name, product.Price);
    }
}
