namespace Sales.Application.Commands.CatalogCommands.ProductCommands;

public sealed class AlterCategoryProductCommand
{
    public Guid ProductId { get; }
    public Guid NewCategoryId { get; }

    public AlterCategoryProductCommand(Guid productId, Guid newCategoryId)
    {
        ProductId = productId;
        NewCategoryId = newCategoryId;
    }
}
