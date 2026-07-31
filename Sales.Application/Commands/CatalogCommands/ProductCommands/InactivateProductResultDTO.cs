namespace Sales.Application.Commands.CatalogCommands.ProductCommands;

public sealed class InactivateProductResultDTO
{
    public Guid ProductId { get; init; }
    public string Status { get; init; } = string.Empty;
}
