namespace Sales.Infrastructure.Commands.Orders;

public sealed class InitPaymentResultDTO
{
    public Guid OrderId { get; init; }
    public Guid PaymentId { get; init; }
    public string OrderStatus { get; init; } = string.Empty;
    public string PaymentStatus { get; init; } = string.Empty;
}
