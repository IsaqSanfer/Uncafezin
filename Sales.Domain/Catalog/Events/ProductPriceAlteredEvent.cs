using Sales.Domain.Common.Base;

namespace Sales.Domain.Catalog.Events;

public sealed record ProductPriceAlteredEvent(Guid ProductId, decimal OldPrice, decimal NewPrice) : DomainEventBase;
