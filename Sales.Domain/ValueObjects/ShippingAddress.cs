using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;
using System.Text.RegularExpressions;

namespace Sales.Domain.ValueObjects;

public class ShippingAddress : ValueObject
{
    public string PostalCode { get; private set; }
    public string Street { get; private set; }
    public string Complement { get; private set; }
    public string District { get; private set; }
    public string State { get; private set; }
    public string City { get; private set; }
    public string Country { get; private set; }

    private ShippingAddress(string postalCode, string street, string complement, string district, string state, string city, string country)
    {
        Guard.AgainstNullOrWhiteSpace(postalCode, nameof(PostalCode));
        Guard.AgainstNullOrWhiteSpace(street, nameof(Street));
        //Guard.AgainstNullOrWhiteSpace(complement, nameof(Complement));
        Guard.AgainstNullOrWhiteSpace(district, nameof(District));
        Guard.AgainstNullOrWhiteSpace(state, nameof(State));
        Guard.AgainstNullOrWhiteSpace(city, nameof(City));
        Guard.AgainstNullOrWhiteSpace(country, nameof(Country));

        // Validando CEP - Brasil
        if (!Regex.IsMatch(postalCode, @"^\d{5}-?\d{3}$"))
            throw new DomainException("CEP inválido. O formato deve ser 00000-000 ou 00000000.");

        PostalCode = postalCode;
        Street = street;
        Complement = complement ?? string.Empty;
        District = district;
        State = state;
        City = city;
        Country = country;
    }

    public static ShippingAddress Create(string postalCode, string street, string complement, string district, string state, string city, string country)
    {
        return new ShippingAddress(postalCode, street, complement, district, state, city, country);
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return PostalCode;
        yield return Street;
        yield return Complement ?? string.Empty;
        yield return District;
        yield return State;
        yield return City;
        yield return Country;
    }

    public string FullAddress()
    {
        return $"{Street}, {Complement} - {District}, {City}/{State}, {Country} - CEP: {PostalCode}";
    }
}
