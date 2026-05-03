using FluentAssertions;
using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Entities;
using Xunit;

namespace Sales.Domain.Tests.Entities;

public class OrderItemTests
{
    // Método auxiliar para criar um OrderItem válido
    private static OrderItem CreateValidOrderItem(decimal price = 10.0m, int qty = 2)
    {
        return new OrderItem(Guid.NewGuid(), "Test Product", price, qty);
    }

    [Fact(DisplayName = "Deve criar um OrderItem válido")]
    public void CreateValidOrderItem_ShouldReturnOrderItem()
    {
        var productId = Guid.NewGuid();
        var productName = "Teclado Mecânico";
        var unitPrice = 250.00m;
        var quantity = 3;

        var item = new OrderItem(productId, productName, unitPrice, quantity);

        item.ProductId.Should().Be(productId);
        item.ProductName.Should().Be(productName);
        item.UnitPrice.Should().Be(unitPrice);
        item.Quantity.Should().Be(quantity);
        item.AppliedDiscount.Should().Be(0);
        item.TotalPrice.Should().Be(unitPrice * quantity);
    }

    [Xunit.Theory(DisplayName = "Não deve criar um OrderItem com dados inválidos")]
    [InlineData("", "Produto A", 10.0, 1, "ProductId inválido.")]
    [InlineData("guid", "", 10.0, 1, "O nome do produto é obrigatório.")]
    [InlineData("guid", "Produto B", 0, 1, "O preço unitário deve ser maior que zero.")]
    [InlineData("guid", "Producto C", 10.0, 0, "A quantidade deve ser maior que zero.")]
    public void CreateInvalidOrderItem_ShouldThrowException(string type, string productName, decimal price, int qty, string msg)
    {
        // Arrange
        var productId = type == "guid" ? Guid.NewGuid() : Guid.Empty;

        // Act
        Action act = () => new OrderItem(productId, productName, price, qty);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(msg);
    }

    [Fact(DisplayName = "Deve aplicar desconto corretamente quando o valor for válido")]
    public void ApplyDiscount_ShouldUpdateAppliedDiscountAndTotalPrice()
    {
        // Arrange
        var item = CreateValidOrderItem(price: 200m, qty: 2);
        var discount = 50m;

        // Act
        item.ApplyDiscount(discount);

        // Assert
        item.AppliedDiscount.Should().Be(discount);
        item.TotalPrice.Should().Be(item.UnitPrice * item.Quantity - discount);
        item.UpdateDate.Should().NotBeNull();
    }

    [Xunit.Theory(DisplayName = "Não deve aplicar desconto inválido")]
    [InlineData(-10, "Desconto não pode ser negativo.")]
    [InlineData(5000, "Desconto não pode ser maior que o preço total.")]
    public void ApplyDiscount_ShouldThrowDomainException_WhenDiscountIsInvalid(decimal discount, string message)
    {
        // Arrange
        var item = CreateValidOrderItem(price: 100m, qty: 2);

        // Act
        Action act = () => item.ApplyDiscount(discount);

        // Assert
        act.Should().Throw<DomainException>().WithMessage(message);
    }

    [Fact(DisplayName = "Deve adicionar quantidade corretamente quando o valor for válido")]
    public void AddQuantity_ShouldUpdateQuantityAndTotalPrice()
    {
        // Arrange
        var item = CreateValidOrderItem(price: 50m, qty: 2);

        // Act
        item.AddQuantity(3);

        // Assert
        item.Quantity.Should().Be(5);
        item.TotalPrice.Should().Be(item.UnitPrice * item.Quantity);
        item.UpdateDate.Should().NotBeNull();
    }

    [Fact(DisplayName = "Não deve adicionar quantidade inválida")]
    public void AddQuantity_ShouldThrowDomainException_WhenUnitsIsInvalid()
    {
        // Arrange
        var item = CreateValidOrderItem();
        // Act
        Action act = () => item.AddQuantity(0);
        // Assert
        act.Should().Throw<DomainException>().WithMessage("A quantidade deve ser maior que zero.");
    }

    [Fact(DisplayName = "Não deve remover quantidade inválida")]
    public void RemoveQuantity_ShouldThrowDomainException_WhenUnitsIsInvalid()
    {
        // Arrange
        var item = CreateValidOrderItem(price: 100m, qty: 5);
        // Act
        Action act = () => item.RemoveQuantity(2);
        // Assert
        act.Should().Throw<DomainException>().WithMessage("A quantidade deve ser maior que zero.");
    }

    [Fact(DisplayName = "Deve atualizar preço corretamente quando o valor for válido")]
    public void UpdateUnitPrice_ShouldUpdateUnitPriceAndTotalPrice()
    {
        // Arrange
        var item = CreateValidOrderItem(price: 100m, qty: 3);

        // Act
        item.UpdateUnitPrice(150m);

        // Assert
        item.UnitPrice.Should().Be(150m);
        item.TotalPrice.Should().Be(item.UnitPrice * item.Quantity);
        item.UpdateDate.Should().NotBeNull();
    }

    [Fact(DisplayName = "Não deve atualizar preço com valor inválido")]
    public void UpdateUnitPrice_ShouldThrowDomainException_WhenPriceIsInvalid()
    {
        // Arrange
        var item = CreateValidOrderItem();

        // Act
        Action act = () => item.UpdateUnitPrice(0);

        // Assert
        act.Should().Throw<DomainException>().WithMessage("O preço unitário deve ser maior que zero.");
    }

    [Fact(DisplayName = "Dois itens com o mesmo ID devem ser considerados iguais")]
    public void TwoItemsWithSameId_ShouldBeEqual()
    {
        // Arrange
        var item1 = CreateValidOrderItem();
        var item2 = CreateValidOrderItem();

        // Forçar o mesmo ID para ambos os itens
        typeof(Entity).GetProperty("Id")!.SetValue(item2, item1.Id);

        // Act & Assert
        (item1 == item2).Should().BeTrue();
        item1.Equals(item2).Should().BeTrue();
    }
}