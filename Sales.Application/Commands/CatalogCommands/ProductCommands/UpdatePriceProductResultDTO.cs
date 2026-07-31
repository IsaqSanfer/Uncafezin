namespace Sales.Application.Commands.CatalogCommands.ProductCommands;

public sealed class UpdatePriceProductResultDTO
{
    public Guid ProductId { get; init; }
    public decimal Price { get; init; }
}
