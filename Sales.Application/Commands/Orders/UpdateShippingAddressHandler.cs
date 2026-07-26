using Sales.Application.Abstractions.Persistence;

namespace Sales.Application.Commands.Orders;

public sealed class UpdateShippingAddressHandler
{
    private readonly IOrderRepository _orderRepository;

    public UpdateShippingAddressHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<UpdateShippingAddressResultDTO> HandleAsync(UpdateShippingAddressCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order == null)
            throw new InvalidOperationException($"Order with ID {command.OrderId} not found.");

        order.UpdateShippingAddress(command.NewShippingAddress);

        await _orderRepository.UpdateAsync(order, cancellationToken);

        return new UpdateShippingAddressResultDTO(
            order.Id,
            order.ShippingAddress.ToString(),
            order.OrderStatus.ToString()
            );
    }
}
