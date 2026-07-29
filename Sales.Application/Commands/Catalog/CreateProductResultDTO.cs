namespace Sales.Application.Commands.Catalog;

public sealed class CreateProductResultDTO
{
    public Guid ProductId { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Status { get; init; } = string.Empty;
}
