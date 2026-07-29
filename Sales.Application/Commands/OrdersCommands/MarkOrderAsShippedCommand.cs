namespace Sales.Application.Commands.OrdersCommands;

public sealed class MarkOrderAsShippedCommand
{
    public Guid OrderId { get; }

    public MarkOrderAsShippedCommand(Guid orderId)
    {
        OrderId = orderId;
    }
}
