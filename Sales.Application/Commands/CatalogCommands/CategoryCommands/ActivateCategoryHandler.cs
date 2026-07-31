using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.CatalogCommands.CategoryCommands;

public sealed class ActivateCategoryHandler
{
    private readonly ICategoryRepository _categoryRepository;

    public ActivateCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<ActivateCategoryResultDTO> HandleAsync(ActivateCategoryCommand command, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(command.CategoryId, cancellationToken);

        if (category is null)
            throw new DomainException($"Category with ID {command.CategoryId} not found.");

        category.Activate();

        await _categoryRepository.UpdateAsync(category, cancellationToken);

        return new ActivateCategoryResultDTO
        {
            CategoryId = category.Id,
            Active = category.Active
        };
    }
}
