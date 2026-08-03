namespace Sales.Domain.Orders.Integration.Customers;

public sealed class AddressDTO
{
    public Guid Id { get; }
    public string PostalCode { get; }
    public string Street { get; }
    public string Number { get; }
    public string District { get; }
    public string City { get; }
    public string State { get; }
    public string Country { get; }
    public string Complement { get; }

    public AddressDTO(
        Guid id,
        string postalCode,
        string street,
        string number,
        string district,
        string city,
        string state,
        string country,
        string complement)
    {
        Id = id;
        PostalCode = postalCode;
        Street = street;
        Number = number;
        District = district;
        City = city;
        State = state;
        Country = country;
        Complement = complement ?? string.Empty;
    }
}
