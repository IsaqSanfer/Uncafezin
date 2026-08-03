using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Orders.Integration.Customers;

public sealed class CustomersACL
{
    private readonly ICustomersGateway _customersGateway;

    public CustomersACL(ICustomersGateway customersGateway)
    {
        Guard.AgainstNull(customersGateway, nameof(customersGateway));
        _customersGateway = customersGateway;
    }

    public async Task<ShippingAddressSnapshot> GetShippingAddressAsync(Guid customerId, Guid shippingId, CancellationToken cancellationToken)
    {
        var shippingAddressDto = await _customersGateway.GetAddressAsync(customerId, shippingId, cancellationToken);

        if (shippingAddressDto is null)
            throw new DomainException($"Shipping address for customer with ID '{customerId}' not found.");

        return new ShippingAddressSnapshot(shippingAddressDto.PostalCode, shippingAddressDto.Street, shippingAddressDto.Number, shippingAddressDto.District, shippingAddressDto.City, shippingAddressDto.State, shippingAddressDto.Country, shippingAddressDto.Complement);
    }
}
