namespace Sales.Application.Commands.OrdersCommands;

public sealed class CreateOrderCommand
{
    public Guid CustomerId { get; }
    public Guid ShippingId { get; }

    public CreateOrderCommand(Guid customerId, Guid shippingId)
    {
        CustomerId = customerId;
        ShippingId = shippingId;
    }
}
