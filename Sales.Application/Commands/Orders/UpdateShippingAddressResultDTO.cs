namespace Sales.Application.Commands.Orders;

public sealed class UpdateShippingAddressResultDTO
{
    public Guid OrderId { get; }
    public string ShippingAddress { get; }
    public string Status { get; }

    public UpdateShippingAddressResultDTO(Guid orderId, string shippingAddress, string status)
    {
        OrderId = orderId;
        ShippingAddress = shippingAddress;
        Status = status;
    }
}
