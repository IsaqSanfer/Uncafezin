using Sales.Domain.Common.Enums;
using Sales.Domain.ValueObjects;

namespace Sales.Domain.Events;

public sealed record OrderCancelledEvent(Guid OrderId, Guid ClientId, OrderStatus Status, CancelReason CancelReason, Guid? PaymentId) : DomainEventBase;
