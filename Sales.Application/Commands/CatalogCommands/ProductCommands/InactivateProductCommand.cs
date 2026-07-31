namespace Sales.Application.Commands.CatalogCommands.ProductCommands;

public sealed class InactivateProductCommand
{
    public Guid ProductId { get; }

    public InactivateProductCommand(Guid productId)
    {
        ProductId = productId;
    }
}
