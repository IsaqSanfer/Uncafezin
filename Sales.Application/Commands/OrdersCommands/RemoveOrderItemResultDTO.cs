namespace Sales.Application.Commands.OrdersCommands;

public sealed class RemoveOrderItemResultDTO
{
    public Guid OrderId { get; }
    public decimal TotalValue { get; }
    public string Status { get; }

    public RemoveOrderItemResultDTO(Guid orderId, decimal totalValue, string status)
    {
        OrderId = orderId;
        TotalValue = totalValue;
        Status = status;
    }
}
