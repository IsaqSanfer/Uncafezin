using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Customers.ValueObjects;

public sealed class CompleteName : ValueObject
{
    public string Name { get; }
    public string LastName { get; }
    public string FullName { get; }

    public CompleteName(string fullName)
    {
        Guard.AgainstNullOrWhiteSpace(fullName, nameof(fullName), "O nome completo é obrigatório.");

        var text = fullName.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        Guard.Against<DomainException>(text.Length < 2, "O nome completo deve conter pelo menos nome e sobrenome.");

        LastName = text.Last();
        Name = string.Join(' ', text.Take(text.Length - 1));

        FullName = string.Join(' ', text);
    }

    public string FirstName => $"{Name.Split(' ').First()} {LastName}";

    public override string ToString()
    {
        return FullName;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return FullName.ToLowerInvariant();
    }
}