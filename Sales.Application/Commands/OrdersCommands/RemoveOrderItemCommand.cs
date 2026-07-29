namespace Sales.Application.Commands.OrdersCommands;

public sealed class RemoveOrderItemCommand
{
    public Guid OrderId { get; }
    public Guid ItemId { get; }
    public decimal TotalValue { get; }
    public string Status { get; }
    public RemoveOrderItemCommand(Guid orderId, Guid itemId, decimal totalValue, string status)
    {
        OrderId = orderId;
        ItemId = itemId;
        TotalValue = totalValue;
        Status = status;
    }
}
