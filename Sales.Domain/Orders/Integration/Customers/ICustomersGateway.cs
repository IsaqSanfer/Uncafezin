namespace Sales.Domain.Orders.Integration.Customers;

public interface ICustomersGateway
{
    Task<AddressDTO?> GetAddressAsync(Guid customerId, Guid addressId, CancellationToken cancellationToken = default);
}
