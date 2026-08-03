using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Orders;
using Sales.Domain.Orders.Integration.Customers;

namespace Sales.Application.Commands.OrdersCommands;

public sealed class CreateOrderHandler
{
    private readonly IOrderRepository _orderRepository;
    private readonly ICustomersGateway _customersGateway;
    private readonly CustomersACL _customersACL;

    public CreateOrderHandler(IOrderRepository orderRepository, ICustomersGateway customersGateway, CustomersACL customersACL)
    {
        _orderRepository = orderRepository;
        _customersGateway = customersGateway;
        _customersACL = customersACL;
    }
    public async Task<CreateOrderResultDTO> HandleAsync(CreateOrderCommand command, CancellationToken cancellationToken = default)
    {
        // Buscar o endereço do cliente usando o gateway de clientes (upstream)
        var customerAddress = await _customersGateway.GetAddressAsync(command.CustomerId, command.ShippingId, cancellationToken);

        if (customerAddress == null)
            throw new InvalidOperationException("Customer address not found.");

        // ACL traduz o DTO de endereço para o snapshot de endereço do domínio
        var address = _customersACL.TranslateAddress(customerAddress);

        // Criar o pedido usando o snapshot de endereço
        var order = Order.Create(command.CustomerId, address);

        // Salvar o pedido no repositório
        await _orderRepository.AddAsync(order, cancellationToken);

        // Retornar o resultado do pedido criado
        return new CreateOrderResultDTO(order.Id, order.OrderCode, order.CreateDate, order.TotalValue, order.OrderStatus.ToString());
    }
}
