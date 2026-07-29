using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;

namespace Sales.Application.Commands.OrdersCommands;

public sealed class MarkOrderAsShippedHandler
{
    private readonly IOrderRepository _orderRepository;

    public MarkOrderAsShippedHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<MarkOrderAsShippedResultDTO> HandleAsync(MarkOrderAsShippedCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);
        if (order is null)
            throw new DomainException($"Pedido com ID {command.OrderId} não encontrado.");

        order.MarkAsShipped();

        await _orderRepository.UpdateAsync(order, cancellationToken);

        return new MarkOrderAsShippedResultDTO
        {
            OrderId = order.Id,
            OrderStatus = order.OrderStatus.ToString()
        };
    }
}
