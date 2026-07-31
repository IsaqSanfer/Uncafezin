namespace Sales.Application.Commands.CatalogCommands.ProductCommands;

public sealed class AlterNameProductCommand
{
    public Guid ProductId { get; }
    public string NewName { get; }

    public AlterNameProductCommand(Guid productId, string newName)
    {
        ProductId = productId;
        NewName = newName;
    }
}
