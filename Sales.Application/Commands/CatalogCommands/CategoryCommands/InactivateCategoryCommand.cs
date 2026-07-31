namespace Sales.Application.Commands.CatalogCommands.CategoryCommands;

public sealed class InactivateCategoryCommand
{
    public Guid CategoryId { get; }

    public InactivateCategoryCommand(Guid categoryId)
    {
        CategoryId = categoryId;
    }
}
