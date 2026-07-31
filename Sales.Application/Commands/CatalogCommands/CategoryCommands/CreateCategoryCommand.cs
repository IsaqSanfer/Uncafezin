namespace Sales.Application.Commands.CatalogCommands.CategoryCommands;

public sealed class CreateCategoryCommand
{
    public string Name { get; }
    public string? Description { get; }

    public CreateCategoryCommand(string name, string? description = null)
    {
        Name = name;
        Description = description;
    }
}
