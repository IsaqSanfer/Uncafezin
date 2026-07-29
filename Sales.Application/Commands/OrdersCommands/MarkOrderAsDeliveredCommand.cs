namespace Sales.Application.Commands.OrdersCommands;

public sealed class MarkOrderAsDeliveredCommand
{
    public Guid OrderId { get; }

    public MarkOrderAsDeliveredCommand(Guid orderId)
    {
        OrderId = orderId;
    }
}
