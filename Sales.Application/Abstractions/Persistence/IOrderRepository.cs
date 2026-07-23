using Sales.Domain.Orders.Entities;

namespace Sales.Application.Abstractions.Persistence;

public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task AddAsync(Order order, CancellationToken cancellationToken = default);
    Task UpdateAsync(Order order, CancellationToken cancellationToken = default);
}

// Seguindo CQRS, o repositório de leitura e escrita são separados.
// O repositório de leitura é responsável por consultas e o repositório de escrita é responsável por comandos.
// Command e Query Responsibility Segregation (CQRS) é um padrão de arquitetura que separa as operações de
// leitura e escrita em diferentes modelos, permitindo otimizações específicas para cada tipo de operação.