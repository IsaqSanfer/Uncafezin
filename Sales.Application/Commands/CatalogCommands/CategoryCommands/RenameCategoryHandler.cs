using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.CatalogCommands.CategoryCommands;

public sealed class RenameCategoryHandler
{
    private readonly ICategoryRepository _categoryRepository;

    public RenameCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<RenameCategoryResultDTO> HandleAsync(RenameCategoryCommand command, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

        if (category is null)
            throw new DomainException($"Category with ID {command.CategoryId} not found.");

        category.AlterName(command.NewName);

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        return new RenameCategoryResultDTO
        {
            CategoryId = category.Id,
            Name = category.Name
        };
    }
}
