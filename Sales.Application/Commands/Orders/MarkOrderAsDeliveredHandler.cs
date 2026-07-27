using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.Orders;

public sealed class MarkOrderAsDeliveredHandler
{
    private readonly IOrderRepository _orderRepository;

    public MarkOrderAsDeliveredHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<MarkOrderAsDeliveredResultDTO> HandleAsync(MarkOrderAsDeliveredCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order == null)
            throw new DomainException("Order not found.");

        order.MarkAsDelivered();

        await _orderRepository.UpdateAsync(order, cancellationToken);

        return new MarkOrderAsDeliveredResultDTO
        {
            OrderId = order.Id,
            OrderStatus = order.OrderStatus.ToString()
        };
    }
}
