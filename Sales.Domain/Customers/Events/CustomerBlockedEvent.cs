using Sales.Domain.Common.Base;

namespace Sales.Domain.Customers.Events;

public sealed record CustomerBlockedEvent(Guid CustomerId, string Cpf) : DomainEventBase;
