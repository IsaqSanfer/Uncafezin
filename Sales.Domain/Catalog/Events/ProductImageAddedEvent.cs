using Sales.Domain.Common.Base;

namespace Sales.Domain.Catalog.Events;

public sealed record ProductImageAddedEvent(Guid ProductId, string ImageUrl, int Position) : DomainEventBase;
