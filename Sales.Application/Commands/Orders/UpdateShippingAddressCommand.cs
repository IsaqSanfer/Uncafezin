using Sales.Domain.Orders.ValueObjects;

namespace Sales.Application.Commands.Orders;

public sealed class UpdateShippingAddressCommand
{
    public Guid OrderId { get; }
    public ShippingAddress ShippingAddress { get; }

    public UpdateShippingAddressCommand(Guid orderId, ShippingAddress newShippingAddress)
    {
        OrderId = orderId;
        ShippingAddress = newShippingAddress;
    }
}
