using Sales.Domain.ValueObjects;

namespace Sales.Domain.Events;

public sealed record OrderShippedEvent(Guid OrderId, Guid ClientId, ShippingAddress ShippingAddress) : DomainEventBase;
