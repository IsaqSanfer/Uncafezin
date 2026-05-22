using Sales.Domain.Common.Base;

namespace Sales.Domain.Customers.Events;

public sealed record CustomerRegisteredEvent(Guid CustomerId, string Name, string Cpf, string Email) : DomainEventBase;
