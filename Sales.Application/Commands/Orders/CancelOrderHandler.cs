using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Orders.ValueObjects;

namespace Sales.Application.Commands.Orders;

public sealed class CancelOrderHandler
{
    private readonly IOrderRepository _orderRepository;

    public CancelOrderHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<CancelOrderResultDTO> HandleAsync(CancelOrderCommand command, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order == null)
            throw new DomainException("Order not found.");

        var reason = new CancelReason(command.ReasonCode);

        order.CancelOrder(reason);

        await _orderRepository.UpdateAsync(order, cancellationToken);

        return new CancelOrderResultDTO
        {
            OrderId = order.Id,
            ReasonCode = order.OrderStatus.ToString()
        };
    }
}
