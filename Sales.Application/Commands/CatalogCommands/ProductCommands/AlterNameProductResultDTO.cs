namespace Sales.Application.Commands.CatalogCommands.ProductCommands;

public sealed class AlterNameProductResultDTO
{
    public Guid ProductId { get; init; }
    public string Name { get; init; } = string.Empty;
}
