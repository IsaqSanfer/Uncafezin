using Sales.Application.Abstractions.Persistence;

namespace Sales.Application.Commands.Orders;

public sealed class AddOrderItemCommandHandler
{
    private readonly IOrderRepository _orderRepository;

    public AddOrderItemCommandHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<AddOrderItemResultDTO> HandleAsync(AddOrderItemCommand command, CancellationToken cancellationToken = default)
    {
        // Procurando o pedido pelo ID fornecido no comando
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order is null)
            throw new InvalidOperationException($"Order with ID {command.OrderId} not found.");

        // Adicionando o item ao pedido
        order.AddItem(command.ProductId, command.ProductName, command.UnitPrice, command.Quantity);
        await _orderRepository.UpdateAsync(order, cancellationToken);

        return new AddOrderItemResultDTO(
            order.Id,
            order.TotalValue,
            order.OrderStatus.ToString()
        );
    }
}
