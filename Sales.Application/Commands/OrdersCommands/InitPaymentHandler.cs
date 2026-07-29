using Sales.Application.Abstractions.Persistence;

namespace Sales.Application.Commands.OrdersCommands;

public sealed class InitPaymentHandler
{
    private readonly IOrderRepository _orderRepository;

    public InitPaymentHandler(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<InitPaymentResultDTO> HandleAsync(InitPaymentCommand command, CancellationToken cancellationToken = default)
    {
        // Procurando o pedido pelo ID fornecido no comando
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order is null)
            throw new InvalidOperationException($"Order with ID {command.OrderId} not found.");

        // Adicionando o pagamento ao pedido usando o método de pagamento fornecido
        var payment = order.AddPayment(command.PaymentMethod);

        await _orderRepository.UpdateAsync(order, cancellationToken);

        return new InitPaymentResultDTO
        {
            OrderId = order.Id,
            PaymentId = payment.Id,
            OrderStatus = order.OrderStatus.ToString(),
            PaymentStatus = payment.PaymentStatus.ToString()
        };
    }
}
