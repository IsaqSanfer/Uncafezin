using Sales.Domain.Orders.Integration.Customers;

namespace Sales.Infrastructure.Gateways;

internal class CustomersGateway : ICustomersGateway
{   
    // Cliente identificado pelo seu Guid, que contém um dicionário de endereços (AddressDTO) identificados por Guid tbm
    private static readonly Dictionary<Guid, Dictionary<Guid, AddressDTO>> _customers = new()
    {
        [new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")] = new Dictionary<Guid, AddressDTO>
        {
            [new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa")] = new AddressDTO(new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa"), "12345", "Main St", "123", "District A", "City A", "State A", "Country A", "Complement A")
        },
        [new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")] = new Dictionary<Guid, AddressDTO>
        {
            [new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb")] = new AddressDTO(new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb"), "67890", "456 Elm St", "456", "District B", "City B", "State B", "Country B", "Complement B")
        },
        [new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc")] = new Dictionary<Guid, AddressDTO>
        {
            [new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc")] = new AddressDTO(new Guid("cccccccc-cccc-cccc-cccc-cccccccccccc"), "54321", "789 Oak St", "789", "District C", "City C", "State C", "Country C", "Complement C")
        }
    };

    public Task<AddressDTO?> GetAddressAsync(Guid customerId, Guid addressId, CancellationToken cancellationToken = default)
    {
        if (_customers.TryGetValue(customerId, out var addresses) && addresses.TryGetValue(addressId, out var address))
            return Task.FromResult<AddressDTO?>(address);

        return Task.FromResult<AddressDTO?>(null);
    }
}
