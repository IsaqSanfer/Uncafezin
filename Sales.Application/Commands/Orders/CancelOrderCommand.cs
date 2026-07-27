namespace Sales.Application.Commands.Orders;

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
