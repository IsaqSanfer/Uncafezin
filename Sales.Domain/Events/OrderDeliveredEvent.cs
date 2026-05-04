namespace Sales.Domain.Events;

public sealed record OrderDeliveredEvent(Guid OrderId, Guid ClientId) : DomainEventBase;