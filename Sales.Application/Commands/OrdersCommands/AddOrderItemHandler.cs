using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Orders.Integration.Catalog;

namespace Sales.Application.Commands.OrdersCommands;

public sealed class AddOrderItemHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICatalogGateway _catalogGateway;
    private readonly CatalogACL _catalogACL;

    public AddOrderItemHandler(IOrderRepository orderRepository, ICatalogGateway catalogGateway, CatalogACL catalogACL)
    {
        _orderRepository = orderRepository;
        _catalogGateway = catalogGateway;
        _catalogACL = catalogACL;
    }

    public async Task<AddOrderItemResultDTO> HandleAsync(AddOrderItemCommand command, CancellationToken cancellationToken = default)
    {
        // Procurando o pedido pelo ID fornecido no comando
        var order = await _orderRepository.GetByIdAsync(command.OrderId, cancellationToken);

        if (order is null)
            throw new InvalidOperationException($"Order with ID {command.OrderId} not found.");

        var product = await _catalogGateway.GetProductByIdAsync(command.ProductId, cancellationToken);

        if (product is null)
            throw new InvalidOperationException($"Product with ID {command.ProductId} not found.");

        var (productName, unitPrice) = _catalogACL.TranslateProduct(product);

        order.AddItem(command.ProductId, productName, unitPrice, command.Quantity);
        await _orderRepository.UpdateAsync(order, cancellationToken);

        return new AddOrderItemResultDTO(
            order.Id,
            order.TotalValue,
            order.OrderStatus.ToString()
        );
    }
}
