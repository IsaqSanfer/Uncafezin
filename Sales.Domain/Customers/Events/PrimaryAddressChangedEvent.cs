namespace Sales.Domain.Customers.Events;

using Sales.Domain.Common.Base;

public sealed record PrimaryAddressChangedEvent(Guid CustomerId, Guid NewAddressId) : DomainEventBase;
