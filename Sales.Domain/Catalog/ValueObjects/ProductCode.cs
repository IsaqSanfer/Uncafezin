using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Catalog.ValueObjects;

public sealed class ProductCode : ValueObject
{
    public string Value { get; }

    public ProductCode(string value)
    {
        Guard.AgainstNullOrWhiteSpace(value, nameof(value), "O código do produto é obrigatório.");
        Guard.Against<DomainException>(value.Length < 3, "O código do produto deve ter pelo menos 3 caracteres.");

        Value = value.Trim().ToUpper();
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
