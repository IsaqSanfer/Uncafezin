using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Catalog.ValueObjects;

public sealed class ProductImage : ValueObject
{
    public string Url { get; set; }
    public int Position { get; set; }

    public ProductImage(string url, int position)
    {
        Guard.AgainstNullOrWhiteSpace(url, nameof(url), "A URL da imagem do produto é obrigatória.");
        Guard.Against<DomainException>(position < 1, "A posição da imagem deve ser maior que 1.");

        Url = url;
        Position = position;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Url;
        yield return Position;
    }
}
