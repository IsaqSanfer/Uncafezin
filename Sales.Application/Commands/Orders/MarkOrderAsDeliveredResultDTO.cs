namespace Sales.Application.Commands.Orders;

public sealed class MarkOrderAsDeliveredResultDTO
{
    public Guid OrderId { get; init; }
    public string OrderStatus { get; init; } = string.Empty;
}
