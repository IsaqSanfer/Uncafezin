using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Catalog;

namespace Sales.Application.Commands.CatalogCommands.CategoryCommands;

public sealed class CreateCategoryHandler
{
    private readonly ICategoryRepository _categoryRepository;

    public CreateCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<CreateCategoryResultDTO> HandleAsync(CreateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        var category = new Category(command.Name, command.Description);

        await _categoryRepository.AddAsync(category, cancellationToken);

        return new CreateCategoryResultDTO
        {
            CategoryId = category.Id,
            Name = category.Name,
            Active = category.Active
        };
    }
}
