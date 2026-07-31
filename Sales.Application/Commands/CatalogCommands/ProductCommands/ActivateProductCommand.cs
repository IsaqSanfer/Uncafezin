namespace Sales.Application.Commands.CatalogCommands.ProductCommands;

public sealed class ActivateProductCommand
{
    public Guid ProductId { get; }

    public ActivateProductCommand(Guid productId)
    {
        ProductId = productId;
    }
}
