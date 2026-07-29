namespace Sales.Application.Commands.OrdersCommands;

public sealed class CancelOrderCommand
{
    public Guid OrderId { get; }
    public string ReasonCode { get; }

    public CancelOrderCommand(Guid orderId, string reasonCode)
    {
        OrderId = orderId;
        ReasonCode = reasonCode;
    }
}
