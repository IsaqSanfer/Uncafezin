namespace Sales.Application.Commands.CatalogCommands.ProductCommands;

public sealed class UpdatePriceProductCommand
{
    public Guid ProductId { get; }
    public decimal NewPrice { get; }

    public UpdatePriceProductCommand(Guid productId, decimal newPrice)
    {
        ProductId = productId;
        NewPrice = newPrice;
    }
}
