namespace Sales.Application.Commands.Orders;

public sealed class AddOrderItemResultDTO
{
    public Guid OrderId { get; }
    public decimal TotalValue { get; }
    public string Status { get; }
    public AddOrderItemResultDTO(Guid orderId, decimal totalValue, string status)
    {
        OrderId = orderId;
        TotalValue = totalValue;
        Status = status;
    }
}
