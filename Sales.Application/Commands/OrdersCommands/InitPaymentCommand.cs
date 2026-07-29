using Sales.Domain.Orders.Enums;

namespace Sales.Application.Commands.OrdersCommands;

public sealed class InitPaymentCommand
{
    public Guid OrderId { get; }
    public PaymentMethod PaymentMethod { get; }

    public InitPaymentCommand(Guid orderId, PaymentMethod paymentMethod)
    {
        OrderId = orderId;
        PaymentMethod = paymentMethod;
    }
}
