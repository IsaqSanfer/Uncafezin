namespace Sales.Application.Commands.CatalogCommands.ProductCommands;

public sealed class AdjustStockResultDTO
{
    public Guid ProductId { get; init; }
    public int Stock { get; init; }
}
