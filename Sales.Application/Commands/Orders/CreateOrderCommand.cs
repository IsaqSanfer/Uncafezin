using Sales.Domain.Orders.ValueObjects;

namespace Sales.Application.Commands.Orders;

public sealed class CreateOrderCommand
{
    public Guid CustomerId { get; }
    public ShippingAddress ShippingAddress { get; }

    public CreateOrderCommand(Guid customerId, ShippingAddress shippingAddress)
    {
        CustomerId = customerId;
        ShippingAddress = shippingAddress;
    }
}
