using Sales.Domain.Orders.Enums;

namespace Sales.Infrastructure.Commands.Orders;

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
