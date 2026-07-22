using Sales.Domain.Common.Base;

namespace Sales.Domain.Catalog.Events;

public sealed record ProductStockAdjustedEvent(Guid ProductId, int Quantity, string Reason) : DomainEventBase;
