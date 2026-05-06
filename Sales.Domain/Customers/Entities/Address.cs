using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;
using System.Text.RegularExpressions;

namespace Sales.Domain.Customers.Entities;

public sealed class Address : Entity
{
    public string PostalCode { get; private set; }
    public string Street { get; private set; }
    public string Number { get; private set; }
    public string District { get; private set; }
    public string City { get; private set; }
    public string State { get; private set; }
    public string Country { get; private set; }
    public string Complement { get; private set; }

    public Address(string postalCode, string street, string number, string district, string city, string state, string country, string complement = "")
    {
        Validate(postalCode, street, number, district, city, state, country);

        PostalCode = postalCode;
        Street = street;
        Number = number;
        District = district;
        City = city;
        State = state;
        Country = country;
        Complement = complement;
    }

    internal void Update(string postalCode, string street, string number, string district, string city, string state, string country, string complement = "")
    {
        Validate(postalCode, street, number, district, city, state, country);

        PostalCode = postalCode;
        Street = street;
        Number = number;
        District = district;
        City = city;
        State = state;
        Country = country;
        Complement = complement;
    }

    private static void Validate(string postalCode, string street, string number, string district, string state, string city, string country) 
    {
        Guard.AgainstNullOrWhiteSpace(postalCode, nameof(PostalCode), "O CEP é obrigatório.");
        Guard.Against<DomainException>(!Regex.IsMatch(postalCode, @"^\d{8}$"), "CEP inválido.");

        Guard.AgainstNullOrWhiteSpace(street, nameof(Street), "O logradouro é obrigatório.");
        Guard.Against<DomainException>(street.Length < 3, "Logradouro muito curto.");

        Guard.AgainstNullOrWhiteSpace(number, nameof(Number), "O número é obrigatório.");
        Guard.Against<DomainException>(number.Length < 1, "Número inválido.");

        Guard.AgainstNullOrWhiteSpace(district, nameof(District), "O bairro é obrigatório.");
        Guard.AgainstNullOrWhiteSpace(city, nameof(City), "A cidade é obrigatória.");
        Guard.AgainstNullOrWhiteSpace(state, nameof(State), "O estado é obrigatório.");
        Guard.AgainstNullOrWhiteSpace(country, nameof(Country), "O país é obrigatório.");
    }

    public string FullAddress()
    {
        return $"{Street}, {Number} - {District}, {City}/{State}, {Country} - CEP: {PostalCode} - Complemento: {Complement}";
    }
}
