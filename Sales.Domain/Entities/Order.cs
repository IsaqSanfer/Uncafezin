using Sales.Domain.Common.Base;
using Sales.Domain.Common.Enums;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;
using Sales.Domain.Events;
using Sales.Domain.ValueObjects;

namespace Sales.Domain.Entities;

public sealed class Order : AggregateRoot
{
    public Guid ClientId { get; private set; }
    public ShippingAddress ShippingAddress { get; private set; }
    public decimal TotalValue { get; private set; }
    public OrderStatus OrderStatus { get; private set; }
    public string OrderCode { get; private set; } = string.Empty;

    // Itens do pedido
    private readonly List<OrderItem> _items = new();
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    // Pagamento do pedido
    private readonly List<Payment> _payments = new();
    public IReadOnlyCollection<Payment> Payments => _payments.AsReadOnly();

    // Construtor 
    private Order(Guid clientId, ShippingAddress shippingAddress)
    {
        Guard.AgainstEmptyGuid(clientId, nameof(clientId), "ClientId Inválido.");
        Guard.AgainstNull(shippingAddress, nameof(shippingAddress), "O endereço de entrega é obrigatório.");

        ClientId = clientId;
        ShippingAddress = shippingAddress;
        OrderStatus = OrderStatus.Pending;
        TotalValue = 0m;

        GenerateOrderCode();
    }

    // Criar um novo pedido
    public static Order Create(Guid clientId, ShippingAddress shippingAddress)
    {
        return new Order(clientId, shippingAddress);
    }

    // Adicionar um item ao pedido
    public void AddItem(Guid productId, string productName, decimal unitPrice, int quantity)
    {
        Guard.Against<DomainException>(OrderStatus != OrderStatus.Pending, "Itens só podem ser adicionados a um pedido que está pendente.");

        var existingItem = _items.FirstOrDefault(i => i.ProductId == productId);
        if (existingItem is not null)
        {
            existingItem.AddQuantity(quantity);
        }
        else
        {
            _items.Add(new OrderItem(productId, productName, unitPrice, quantity));
        }

        CalculateTotalValue();
        SetUpdateDate();
    }

    // Remover um item do pedido
    public void RemoveItem(Guid productId)
    {
        Guard.AgainstEmptyGuid(productId, nameof(productId), "ProductId Inválido.");
        Guard.Against<DomainException>(OrderStatus != OrderStatus.Pending, "Itens só podem ser removidos de um pedido que está pendente.");

        var item = _items.FirstOrDefault(i => i.ProductId == productId);
        Guard.AgainstNull(item, nameof(productId), "Item não encontrado no pedido.");

        _items.Remove(item!);   // O operador ! é usado aqui porque já garantimos que item não é nulo antes

        Guard.Against<DomainException>(_items.Count == 0, "O pedido deve conter pelo menos um item.");

        CalculateTotalValue();
        SetUpdateDate();
    }

    // Calcular o valor total do pedido
    private void CalculateTotalValue() => TotalValue = _items.Sum(i => i.TotalPrice);

    // Atualizar o endereço de entrega do pedido
    public void UpdateShippingAddress(ShippingAddress newAddress)
    {
        Guard.AgainstNull(newAddress, nameof(newAddress), "O endereço de entrega é obrigatório.");
        Guard.Against<DomainException>(OrderStatus != OrderStatus.Pending, "O endereço de entrega só pode ser alterado enquanto o pedido está pendente.");

        ShippingAddress = newAddress;
        SetUpdateDate();
    }

    // Adicionar um pagamento ao pedido
    public Payment AddPayment(PaymentMethod paymentMethod)
    {
        Guard.Against<DomainException>(!_items.Any(), "Não é possível adicionar um pagamento a um pedido sem itens.");
        Guard.Against<DomainException>(OrderStatus != OrderStatus.Pending, "Pagamentos só podem ser adicionados a um pedido que está pendente.");

        if (_payments.Any(p => p.PaymentStatus == PaymentStatus.Pending))
            throw new DomainException("Já existe um pagamento pendente para este pedido.");

        var newPayment = new Payment(Id, paymentMethod, TotalValue);
        _payments.Add(newPayment);
        SetUpdateDate();

        // O próprio pagamento emitirá os eventos de domínio (PaymentApproved, PaymentRejected, etc.)
        return newPayment;
    }

    // Lidar com a aprovação do pagamento, que será chamado quando o evento de pagamento aprovado for emitido
    public void HandlePaymentApproved(Guid paymentId)
    {
        var payment = _payments.FirstOrDefault(p => p.Id == paymentId);
        if (payment is null) return;

        Guard.Against<DomainException>(payment.PaymentStatus != PaymentStatus.Pending, "O pedido não está no status esperado para confirmação de pagamento.");

        OrderStatus = OrderStatus.Confirmed;
        SetUpdateDate();
    }

    // Lidar com a rejeição do pagamento, que será chamado quando o evento de pagamento rejeitado for emitido
    public void HandlePaymentRejected(Guid paymentId)
    {
        var payment = _payments.FirstOrDefault(p => p.Id == paymentId);
        if (payment is null) return;

        Guard.Against<DomainException>(payment.PaymentStatus != PaymentStatus.Pending, "O pedido não está no status esperado para rejeição de pagamento.");

        OrderStatus = OrderStatus.Cancelled;
        SetUpdateDate();

        AddDomainEvent(new OrderCancelledEvent(Id, ClientId, OrderStatus, CancelReason.ErroPagamento(), paymentId));
    }

    // Colocar o pedido no status Em separação
    public void MarkAsInSeparation()
    {
        Guard.Against<DomainException>(OrderStatus != OrderStatus.Confirmed, "O pedido só pode ser colocado em separação com o pagamento confirmado.");

        OrderStatus = OrderStatus.Processing;
        SetUpdateDate();
    }

    // Colocar o pedido no status Enviado
    public void MarkAsShipped()
    {
        Guard.Against<DomainException>(OrderStatus != OrderStatus.Processing, "O pedido só pode ser enviado se estiver em separação.");

        OrderStatus = OrderStatus.Shipped;
        SetUpdateDate();

        AddDomainEvent(new OrderShippedEvent(Id, ClientId, ShippingAddress));
    }

    // Colocar o pedido no status Entregue
    public void MarkAsDelivered()
    {
        Guard.Against<DomainException>(OrderStatus != OrderStatus.Shipped, "O pedido só pode ser marcado como entregue se estiver no status Enviado.");

        OrderStatus = OrderStatus.Delivered;
        SetUpdateDate();

        AddDomainEvent(new OrderDeliveredEvent(Id, ClientId));
    }

    // Cancelar o pedido
    public void CancelOrder(CancelReason? reason = null)
    {
        Guard.Against<DomainException>(OrderStatus >= OrderStatus.Processing, "Não é possível cancelar um pedido que já está em separação ou posterior.");

        //Guard.Against<DomainException>(OrderStatus == OrderStatus.Cancelled, "O pedido já está cancelado.");
        //Guard.Against<DomainException>(OrderStatus == OrderStatus.Delivered, "Não é possível cancelar um pedido que já foi entregue.");

        OrderStatus = OrderStatus.Cancelled;
        SetUpdateDate();

        AddDomainEvent(new OrderCancelledEvent(Id, ClientId, OrderStatus, reason ?? CancelReason.Outro(), _payments.LastOrDefault()?.Id));
    }

    // Mock para geração do código do pedido, pode ser implementado um serviço para isso
    public void GenerateOrderCode() => OrderCode = $"CAF-{Id.ToString()[..8].ToUpper()}";

}
