namespace Sales.Application.Commands.CatalogCommands.CategoryCommands;

public sealed class ActivateCategoryCommand
{
    public Guid CategoryId { get; }

    public ActivateCategoryCommand(Guid categoryId)
    {
        CategoryId = categoryId;
    }
}
