using Sales.Domain.Common.Base;

namespace Sales.Domain.Orders.Events;

public sealed record OrderDeliveredEvent(Guid OrderId, Guid ClientId) : DomainEventBase;