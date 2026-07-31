namespace Sales.Application.Commands.CatalogCommands.CategoryCommands;

public sealed class RenameCategoryResultDTO
{
    public Guid CategoryId { get; init; }
    public string Name { get; init; } = string.Empty;
}
