using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Catalog.ValueObjects;

public sealed class ProductPrice : ValueObject
{
    public decimal Value { get; }

    public ProductPrice(decimal value)
    {
        Guard.Against<DomainException>(!(value > 0), "O preço do produto deve ser maior que zero.");

        Value = value;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
