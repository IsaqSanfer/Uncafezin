namespace Sales.Application.Commands.OrdersCommands;

public sealed class MarkOrderAsDeliveredResultDTO
{
    public Guid OrderId { get; init; }
    public string OrderStatus { get; init; } = string.Empty;
}
