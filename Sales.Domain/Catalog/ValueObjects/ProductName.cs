using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Catalog.ValueObjects;

public sealed class ProductName : ValueObject
{
    public string Value { get; }

    public ProductName(string value)
    {
        Guard.AgainstNullOrWhiteSpace(value, nameof(value), "O nome do produto é obrigatório.");
        Guard.Against<DomainException>(value.Length < 3, "O nome do produto deve ter pelo menos 3 caracteres.");
        Guard.Against<DomainException>(value.Length > 100, "O nome do produto não pode ter mais de 100 caracteres.");

        Value = value.Trim();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
