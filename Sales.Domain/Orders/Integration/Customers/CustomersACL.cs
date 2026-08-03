using Sales.Domain.Orders.ValueObjects;

namespace Sales.Domain.Orders.Integration.Customers;

public sealed class CustomersACL
{
    public ShippingAddress TranslateAddress(AddressDTO address)
    {
        return ShippingAddress.Create(
            address.PostalCode,
            address.Street,
            address.Number,
            address.District,
            address.State,
            address.City,
            address.Country,
            address.Complement
        );
    }
}
