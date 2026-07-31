namespace Sales.Application.Commands.CatalogCommands.CategoryCommands;

public sealed class CreateCategoryResultDTO
{
    public Guid CategoryId { get; init; }
    public string Name { get; init; } = string.Empty;
    public bool Active { get; init; }
}
