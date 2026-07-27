namespace Sales.Application.Commands.Orders;

public sealed class MarkOrderAsShippedCommand
{
    public Guid OrderId { get; }

    public MarkOrderAsShippedCommand(Guid orderId)
    {
        OrderId = orderId;
    }
}
