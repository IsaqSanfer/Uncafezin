namespace Sales.Application.Commands.CatalogCommands.CategoryCommands;

public sealed class ActivateCategoryResultDTO
{
    public Guid CategoryId { get; init; }
    public bool Active { get; init; }
}
