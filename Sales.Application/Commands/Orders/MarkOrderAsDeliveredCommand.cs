namespace Sales.Application.Commands.Orders;

public sealed class MarkOrderAsDeliveredCommand
{
    public Guid OrderId { get; }

    public MarkOrderAsDeliveredCommand(Guid orderId)
    {
        OrderId = orderId;
    }
}
