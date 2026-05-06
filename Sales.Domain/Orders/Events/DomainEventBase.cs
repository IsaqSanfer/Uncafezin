namespace Sales.Domain.Orders.Events;

public abstract record class DomainEventBase : IDomainEvent
{
    public DateTime DateOccurred { get; protected set; } = DateTime.UtcNow;
}
