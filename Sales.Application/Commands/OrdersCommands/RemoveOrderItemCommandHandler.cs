using Sales.Application.Abstractions.Persistence;

namespace Sales.Application.Commands.OrdersCommands;

public sealed class RemoveOrderItemCommandHandler
{
    private readonly IOrderRepository _orderRepository;

    public RemoveOrderItemCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<RemoveOrderItemResultDTO> HandleAsync(RemoveOrderItemCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order is null)
            throw new InvalidOperationException($"Order with ID {command.OrderId} not found.");

        order.RemoveItem(command.ItemId);
        await _orderRepository.UpdateAsync(order, cancellationToken);

        return new RemoveOrderItemResultDTO(
            order.Id,
            order.TotalValue,
            order.OrderStatus.ToString()
        );
    }
}
