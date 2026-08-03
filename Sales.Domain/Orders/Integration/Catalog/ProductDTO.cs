namespace Sales.Domain.Orders.Integration.Catalog;

public sealed class ProductDTO
{
    public Guid Id { get; }
    public string Name { get; }
    public decimal Price { get; }

    public ProductDTO(Guid id, string name, decimal price)
    {
        Id = id;
        Name = name;
        Price = price;
    }
}
