namespace Sales.Domain.Orders.Events;

public interface IDomainEvent
{
    DateTime DateOccurred { get; }
}
