namespace Sales.Application.Commands.CatalogCommands.ProductCommands;

public sealed class AdjustStockCommand
{
    public Guid ProductId { get; }
    public int Quantity { get; }
    public string Reason { get; }

    public AdjustStockCommand(Guid productId, int quantity, string reason)
    {
        ProductId = productId;
        Quantity = quantity;
        Reason = reason;
    }
}
