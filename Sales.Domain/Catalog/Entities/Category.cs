using Sales.Domain.Catalog.Events;
using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Catalog.Entities;

public sealed class Category : AggregateRoot
{
    public string Name { get; private set; }
    public string? Description { get; private set; }
    public bool Active { get; private set; }

    public Category(string name, string? description = null)
    {
        Guard.AgainstNullOrWhiteSpace(name, nameof(name), "Nome é obrigatório.");
        Guard.Against<DomainException>(name.Length < 3, "Nome deve ter no mínimo 3 caracteres.");

        Name = name.Trim();
        Description = description;
        Active = true;
    }

    public void AlterName(string newName)
    {
        Guard.AgainstNullOrWhiteSpace(newName, nameof(newName), "Nome é obrigatório.");
        Guard.Against<DomainException>(newName.Length < 3, "Nome deve ter no mínimo 3 caracteres.");

        Name = newName.Trim();
        SetUpdateDate();
    }  

    public void AlterDescription(string? newDescription)
    {
        Description = newDescription;
        SetUpdateDate();
    }

    public void Activate()
    {
        Guard.Against<DomainException>(Active, "Categoria já está ativa.");

        Active = true;
        SetUpdateDate();
        AddDomainEvent(new CategoryActivatedEvent(Id));
    }

    public void Inactivate()
    {
        Guard.Against<DomainException>(!Active, "Categoria já está inativa.");

        Active = false;
        SetUpdateDate();
        AddDomainEvent(new CategoryInactivatedEvent(Id));
    }
}
