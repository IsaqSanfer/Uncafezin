namespace Sales.Application.Commands.OrdersCommands;

public sealed class CancelOrderResultDTO
{
    public Guid OrderId { get; init; }
    public string ReasonCode { get; init; } = string.Empty;
}
