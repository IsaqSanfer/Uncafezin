using Sales.Domain.Common.Base;

namespace Sales.Domain.Orders.Events;

public record PaymentApprovedEvent(Guid PaymentId, Guid OrderId, decimal Value, DateTime PaymentDate, string? CodeTransaction) : DomainEventBase;