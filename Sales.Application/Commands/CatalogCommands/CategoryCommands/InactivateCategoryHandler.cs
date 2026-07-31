using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.CatalogCommands.CategoryCommands;

public sealed class InactivateCategoryHandler
{
    private readonly ICategoryRepository _categoryRepository;

    public InactivateCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<InactivateCategoryResultDTO> HandleAsync(InactivateCategoryCommand command, CancellationToken cancellationToken = default)
    {
        var category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

        if (category is null)
            throw new DomainException($"Category with ID {command.CategoryId} not found.");

        category.Inactivate();

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        return new InactivateCategoryResultDTO
        {
            CategoryId = category.Id,
            Active = category.Active
        };
    }
}
