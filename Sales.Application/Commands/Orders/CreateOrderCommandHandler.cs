using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Orders.Entities;

namespace Sales.Application.Commands.Orders;

public sealed class CreateOrderCommandHandler
{
    private readonly IOrderRepository _orderRepository;

    public CreateOrderCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }
    public async Task<CreateOrderResultDTO> HandleAsync(CreateOrderCommand command, CancellationToken cancellationToken = default)
    {
        var order = Order.Create(command.CustomerId, command.ShippingAddress);

        await _orderRepository.AddAsync(order, cancellationToken);

        return new CreateOrderResultDTO(
            order.Id,
            order.OrderCode,
            order.CreateDate,
            order.TotalValue,
            order.OrderStatus.ToString()
        );
    }
}
