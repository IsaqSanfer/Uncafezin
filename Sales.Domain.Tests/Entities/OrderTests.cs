using FluentAssertions;
using Sales.Domain.Common.Enums;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Entities;
using Sales.Domain.ValueObjects;
using Xunit;

public class OrderTests
{
    // Criação do Pedido

    [Fact(DisplayName = "Deve criar um pedido com as propriedades corretas")]
    public void Create_ShouldReturnOrderWithCorrectProperties()
    {
        // Arrange
        var clientId = Guid.NewGuid();
        var address = ShippingAddress.Create("12345-678", "Rua A", "Apto 1", "Centro", "SP", "São Paulo", "Brasil");

        // Act
        var order = Order.Create(clientId, address);

        // Assert
        order.Should().NotBeNull();
        order.ClientId.Should().Be(clientId);
        order.ShippingAddress.Should().Be(address);
        order.Items.Should().BeEmpty();
        order.Payments.Should().BeEmpty();
    }

    // Itens do Pedido

    [Fact(DisplayName = "Deve retornar lista vazia quando nenhum item for adicionado")]
    public void Items_ShouldReturnEmpty_WhenNoItemsAdded()
    {
        // Arrange
        var address = ShippingAddress.Create("12345-678", "Rua A", "Apto 1", "Centro", "SP", "São Paulo", "Brasil");
        var order = Order.Create(Guid.NewGuid(), address);

        // Act
        var items = order.Items;

        // Assert
        items.Should().BeEmpty();
    }

    [Fact(DisplayName = "Deve adicionar um novo item ao pedido")]
    public void AddItem_ShouldAddNewItemToOrder()
    {
        // Arrange
        var address = ShippingAddress.Create("12345-678", "Rua A", "Apto 1", "Centro", "SP", "São Paulo", "Brasil");
        var order = Order.Create(Guid.NewGuid(), address);
        var productId = Guid.NewGuid();
        var productName = "Produto Teste";
        var unitPrice = 10.5m;
        var quantity = 2;

        // Act
        order.AddItem(productId, productName, unitPrice, quantity);

        // Assert
        order.Items.Should().HaveCount(1);
        var item = order.Items.First();
        item.ProductId.Should().Be(productId);
        item.ProductName.Should().Be(productName);
        item.UnitPrice.Should().Be(unitPrice);
        item.Quantity.Should().Be(quantity);
    }

    [Fact(DisplayName = "Deve somar a quantidade quando o produto já existir no pedido")]
    public void AddItem_ShouldIncreaseQuantity_WhenProductAlreadyExists()
    {
        // Arrange
        var address = ShippingAddress.Create("12345-678", "Rua A", "Apto 1", "Centro", "SP", "São Paulo", "Brasil");
        var order = Order.Create(Guid.NewGuid(), address);
        var productId = Guid.NewGuid();
        var productName = "Produto Teste";
        var unitPrice = 10.5m;
        var quantity = 2;
        order.AddItem(productId, productName, unitPrice, quantity);

        // Act
        order.AddItem(productId, productName, unitPrice, 3);

        // Assert
        order.Items.Should().HaveCount(1);
        var item = order.Items.First();
        item.Quantity.Should().Be(5);
    }

    [Fact(DisplayName = "Deve remover o item correto quando houver múltiplos itens")]
    public void RemoveItem_ShouldRemoveCorrectItem_WhenMultipleItemsExist()
    {
        // Arrange
        var address = ShippingAddress.Create("12345-678", "Rua A", "Apto 1", "Centro", "SP", "São Paulo", "Brasil");
        var order = Order.Create(Guid.NewGuid(), address);
        var productId1 = Guid.NewGuid();
        var productId2 = Guid.NewGuid();
        order.AddItem(productId1, "Produto 1", 10.5m, 2);
        order.AddItem(productId2, "Produto 2", 5.0m, 1);

        // Act
        order.RemoveItem(productId1);

        // Assert
        order.Items.Should().HaveCount(1);
        order.Items.First().ProductId.Should().Be(productId2);
    }

    [Fact(DisplayName = "Deve lançar DomainException ao tentar remover o último item do pedido")]
    public void RemoveItem_ShouldThrowDomainException_WhenRemovingLastItem()
    {
        // Arrange
        var address = ShippingAddress.Create("12345-678", "Rua A", "Apto 1", "Centro", "SP", "São Paulo", "Brasil");
        var order = Order.Create(Guid.NewGuid(), address);
        var productId = Guid.NewGuid();
        order.AddItem(productId, "Produto Teste", 10.5m, 2);

        // Act
        var act = () => order.RemoveItem(productId);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O pedido deve conter pelo menos um item.");
    }

    // Endereço de Entrega

    [Fact(DisplayName = "Deve atualizar endereço quando o pedido estiver Pendente e o endereço for válido")]
    public void UpdateShippingAddress_ShouldUpdate_WhenOrderIsPendingAndAddressIsValid()
    {
        // Arrange
        var address1 = ShippingAddress.Create("12345-678", "Rua A", "Apto 1", "Centro", "SP", "São Paulo", "Brasil");
        var address2 = ShippingAddress.Create("54321-000", "Rua B", "Casa", "Bairro", "RJ", "Rio de Janeiro", "Brasil");
        var order = Order.Create(Guid.NewGuid(), address1);

        // Act
        order.UpdateShippingAddress(address2);

        // Assert
        order.ShippingAddress.Should().Be(address2);
    }

    [Fact(DisplayName = "Deve lançar erro ao tentar atualizar com endereço nulo")]
    public void UpdateShippingAddress_ShouldThrow_WhenAddressIsNull()
    {
        // Arrange
        var address = ShippingAddress.Create("12345-678", "Rua A", "Apto 1", "Centro", "SP", "São Paulo", "Brasil");
        var order = Order.Create(Guid.NewGuid(), address);

        // Act
        var act = () => order.UpdateShippingAddress(null!);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O endereço de entrega é obrigatório.");
    }

    [Fact(DisplayName = "Deve lançar erro ao tentar atualizar endereço se o pedido não estiver Pendente")]
    public void UpdateShippingAddress_ShouldThrow_WhenOrderIsNotPending()
    {
        // Arrange
        var address1 = ShippingAddress.Create("12345-678", "Rua A", "Apto 1", "Centro", "SP", "São Paulo", "Brasil");
        var address2 = ShippingAddress.Create("54321-000", "Rua B", "Casa", "Bairro", "RJ", "Rio de Janeiro", "Brasil");
        var order = Order.Create(Guid.NewGuid(), address1);
        // Simula mudança de status (reflexão, pois não há setter público)
        var statusProp = typeof(Order).GetProperty("OrderStatus");
        statusProp!.SetValue(order, OrderStatus.Confirmed);

        // Act
        var act = () => order.UpdateShippingAddress(address2);

        // Assert
        act.Should().Throw<DomainException>()
            .WithMessage("O endereço de entrega só pode ser alterado enquanto o pedido está pendente.");
    }

    // Pagamentos

    [Fact(DisplayName = "Deve retornar lista vazia quando nenhum pagamento for adicionado")]
    public void Payments_ShouldReturnEmpty_WhenNoPaymentsAdded()
    {
        // Arrange
        var address = ShippingAddress.Create("12345-678", "Rua A", "Apto 1", "Centro", "SP", "São Paulo", "Brasil");
        var order = Order.Create(Guid.NewGuid(), address);

        // Act
        var payments = order.Payments;

        // Assert
        payments.Should().BeEmpty();
    }

    // Transição de Status

    [Fact(DisplayName = "Deve alterar status para Enviado e disparar evento quando pedido estiver Em Processamento")]
    public void MarkAsShipped_ShouldSetStatusAndRaiseEvent_WhenOrderIsProcessing()
    {
        // Arrange
        var address = ShippingAddress.Create("12345-678", "Rua A", "Apto 1", "Centro", "SP", "São Paulo", "Brasil");
        var order = Order.Create(Guid.NewGuid(), address);
        var statusProp = typeof(Order).GetProperty("OrderStatus");
        statusProp!.SetValue(order, OrderStatus.Processing);

        // Act
        order.MarkAsShipped();

        // Assert
        var newStatus = statusProp.GetValue(order);
        newStatus.Should().Be(OrderStatus.Shipped);
        // Verifica se evento foi adicionado
        var events = order.DomainEvents;
        events.Should().NotBeNull();
        events.Should().Contain(e => e.GetType().Name == "OrderShippedEvent");
    }

    // Cancelamento
    // ...
}
