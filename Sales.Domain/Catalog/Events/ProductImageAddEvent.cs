using Sales.Domain.Common.Base;

namespace Sales.Domain.Catalog.Events;

public sealed record ProductImageAddEvent(Guid ProductId, string ImageUrl, int Position) : DomainEventBase;
