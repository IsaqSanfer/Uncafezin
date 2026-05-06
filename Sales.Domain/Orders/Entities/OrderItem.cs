using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Orders.Entities;

public sealed class OrderItem : Entity
{
    public Guid ProductId { get; private set; }
    public string ProductName { get; private set; } = string.Empty;
    public decimal UnitPrice { get; private set; }
    public int Quantity { get; private set; }
    public decimal AppliedDiscount { get; private set; }
    public decimal TotalPrice { get; private set; }
    internal OrderItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        Guard.AgainstEmptyGuid(productId, nameof(productId), "ProductId inválido.");
        Guard.AgainstNullOrWhiteSpace(productName, nameof(productName), "O nome do produto é obrigatório.");
        Guard.Against<DomainException>(unitPrice <= 0, "O preço unitário deve ser maior que zero.");
        Guard.Against<DomainException>(quantity <= 0, "A quantidade deve ser maior que zero.");

        ProductId = productId;
        ProductName = productName;
        UnitPrice = unitPrice;
        Quantity = quantity;
        AppliedDiscount = 0;
        CalculateTotalPrice();
    }
    public void ApplyDiscount(decimal discountAmount)
    {
        Guard.Against<DomainException>(discountAmount < 0, "O desconto não pode ser negativo.");
        Guard.Against<DomainException>(discountAmount > UnitPrice * Quantity, "O desconto não pode exceder o valor total do item.");

        AppliedDiscount = discountAmount;
        SetUpdateDate();
        CalculateTotalPrice();
    }
    public void AddQuantity(int units)
    {
        Guard.Against<DomainException>(units <= 0, "A quantidade deve ser maior que zero.");

        Quantity += units;
        SetUpdateDate();
        CalculateTotalPrice();
    }
    public void RemoveQuantity(int units)
    {
        Guard.Against<DomainException>(units <= 0, "A quantidade deve ser maior que zero.");
        Guard.Against<DomainException>(units > Quantity, "Não é possível remover mais unidades do que a quantidade atual.");

        Quantity -= units;
        SetUpdateDate();
        CalculateTotalPrice();
    }
    public void UpdateUnitPrice(decimal newUnitPrice)
    {
        Guard.Against<DomainException>(newUnitPrice <= 0, "O preço unitário deve ser maior que zero.");

        UnitPrice = newUnitPrice;
        SetUpdateDate();
        CalculateTotalPrice();
    }
    private void CalculateTotalPrice()
    {
        TotalPrice = (UnitPrice * Quantity) - AppliedDiscount;
    }
}
