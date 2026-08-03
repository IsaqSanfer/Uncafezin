using Sales.Domain.Common.Base;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Orders.Integration.Customers;

public sealed class ShippingAddressSnapshot : ValueObject
{
    public string PostalCode { get; }
    public string Street { get; }
    public string Number { get; }
    public string District { get; }
    public string City { get; }
    public string State { get; }
    public string Country { get; }
    public string Complement { get; }

    public ShippingAddressSnapshot(
        string postalCode,
        string street,
        string number,
        string district,
        string city,
        string state,
        string country,
        string complement)
    {
        Guard.AgainstNullOrWhiteSpace(postalCode, nameof(postalCode), "CEP é obrigatório.");
        Guard.AgainstNullOrWhiteSpace(street, nameof(street), "Rua é obrigatória.");
        Guard.AgainstNullOrWhiteSpace(number, nameof(number), "Número é obrigatório.");
        Guard.AgainstNullOrWhiteSpace(district, nameof(district), "Bairro é obrigatório.");
        Guard.AgainstNullOrWhiteSpace(city, nameof(city), "Cidade é obrigatória.");
        Guard.AgainstNullOrWhiteSpace(state, nameof(state), "Estado é obrigatório.");
        Guard.AgainstNullOrWhiteSpace(country, nameof(country), "País é obrigatório.");

        PostalCode = postalCode;
        Street = street;
        Number = number;
        District = district;
        City = city;
        State = state;
        Country = country;
        Complement = complement ?? string.Empty;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PostalCode;
        yield return Street;
        yield return Number;
        yield return District;
        yield return City;
        yield return State;
        yield return Country;
        yield return Complement;
    }
}
