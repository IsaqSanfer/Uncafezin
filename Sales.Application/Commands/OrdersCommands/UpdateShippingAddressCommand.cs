using Sales.Domain.Orders.ValueObjects;

namespace Sales.Application.Commands.OrdersCommands;

public sealed class UpdateShippingAddressCommand
{
    public Guid OrderId { get; }
    public ShippingAddress NewShippingAddress { get; }

    public UpdateShippingAddressCommand(Guid orderId, ShippingAddress newShippingAddress)
    {
        OrderId = orderId;
        NewShippingAddress = newShippingAddress;
    }
}
