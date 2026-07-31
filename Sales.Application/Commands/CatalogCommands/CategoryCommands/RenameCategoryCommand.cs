namespace Sales.Application.Commands.CatalogCommands.CategoryCommands;

public sealed class RenameCategoryCommand
{
    public Guid CategoryId { get; }
    public string NewName { get; }

    public RenameCategoryCommand(Guid categoryId, string newName)
    {
        CategoryId = categoryId;
        NewName = newName;
    }
}
