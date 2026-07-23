namespace Sales.Application.Commands.Orders;

public sealed class CreateOrderResultDTO
{
    public Guid OrderId { get; }
    public string OrderCode { get; }
    public DateTime CreateDate { get; }
    public decimal TotalValue { get; }
    public string Status { get; }

    public CreateOrderResultDTO(Guid orderId, string orderCode, DateTime createDate, decimal totalValue, string status)
    {
        OrderId = orderId;
        OrderCode = orderCode;
        CreateDate = createDate;
        TotalValue = totalValue;
        Status = status;
    }
}
