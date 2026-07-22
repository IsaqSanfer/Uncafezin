using Sales.Domain.Common.Base;

namespace Sales.Domain.Catalog.Events;

public sealed record CategoryInactivatedEvent(Guid CategoryId) : DomainEventBase;