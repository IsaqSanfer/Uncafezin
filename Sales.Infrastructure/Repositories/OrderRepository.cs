using Sales.Application.Abstractions.Persistence;
using Sales.Domain.Orders;

namespace Sales.Infrastructure.Repositories;

internal class OrderRepository : IOrderRepository
{
    private readonly Dictionary<Guid, Order> _orders = new();
    private readonly SemaphoreSlim _lock = new(1,1);    // Serve para garantir que apenas uma thread acesse o dicionário de pedidos por vez, evitando problemas de concorrência.

    public async Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);

        try
        {
            _orders.TryGetValue(orderId, out var order);
            return order;
        }
        finally
        {
            _lock.Release();    // Libera o lock para que outras threads possam acessar o dicionário de pedidos.
        }
    }

    public async Task AddAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);

        try
        {
            _orders[order.Id] = order;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task UpdateAsync(Order order, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);

        try
        {
            _orders[order.Id] = order;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<IReadOnlyList<Order>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            return _orders.Values.ToList().AsReadOnly();
        }
        finally
        {
            _lock.Release();
        }
    }
}
