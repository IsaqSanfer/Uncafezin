using Sales.Domain.Common.Base;
using Sales.Domain.Common.Enums;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;
using Sales.Domain.Orders.Events;

namespace Sales.Domain.Orders.Entities;

public sealed class Payment : Entity
{
    public Guid OrderId { get; private set; }
    public PaymentMethod PaymentMethod { get; private set; }
    public PaymentStatus PaymentStatus { get; private set; }
    public decimal Value { get; private set; }
    public DateTime? PaymentDate { get; private set; }
    public string? CodeTransaction { get; private set; }

    public Payment(Guid orderId, PaymentMethod paymentMethod, decimal value)
    {
        Guard.AgainstEmptyGuid(orderId, nameof(orderId), "Pedido inválido.");
        Guard.Against<DomainException>(value <= 0, "Valor do pagamento deve ser maior que zero.");
        Guard.Against<DomainException>(!Enum.IsDefined(typeof(PaymentMethod), paymentMethod), "Método de pagamento inválido.");

        OrderId = orderId;
        PaymentMethod = paymentMethod;
        Value = value;

        // Status inicial do pagamento
        PaymentStatus = PaymentStatus.Pending;
        PaymentDate = null;
        CodeTransaction = null;
    }

    // Mock para geração do código de transação (poderia ser obtido através de um gateway externo)
    public void GenerateInternalTransactionCode()
    {
        if (CodeTransaction is not null)
            return;     //throw new DomainException("Código de transação já gerado.");

        var code = $"LOCAL-{Guid.NewGuid().ToString()[..8].ToUpper()}";
        SetTransactionCode(code);
    }

    public void SetTransactionCode(string code)
    {
        Guard.AgainstNullOrWhiteSpace(code, nameof(code), "Código de transação é obrigatório.");
        Guard.Against<DomainException>(CodeTransaction is not null, "Código de transação já definido.");
        Guard.Against<DomainException>(PaymentStatus != PaymentStatus.Pending, "Não é possível definir o código de transação para um pagamento que não está pendente.");

        // Gerado apenas uma vez, quando o pagamento é aprovado
        CodeTransaction = code;
        SetUpdateDate();
    }

    public void ConfirmPayment()
    {
        Guard.Against<DomainException>(PaymentStatus != PaymentStatus.Pending, "Apenas pagamentos pendentes podem ser confirmados.");

        Guard.AgainstNullOrWhiteSpace(CodeTransaction ?? string.Empty, nameof(CodeTransaction), "Código de transação é obrigatório para confirmar o pagamento.");

        PaymentStatus = PaymentStatus.Approved;
        PaymentDate = DateTime.UtcNow;
        SetUpdateDate();

        AddDomainEvent(new PaymentApprovedEvent(Id, OrderId, Value, PaymentDate.Value, CodeTransaction));
    }

    public void RejectPayment()
    {
        Guard.Against<DomainException>(PaymentStatus != PaymentStatus.Pending, "Apenas pagamentos pendentes podem ser recusados.");

        PaymentStatus = PaymentStatus.Refused;
        PaymentDate = DateTime.UtcNow;
        SetUpdateDate();

        AddDomainEvent(new PaymentRejectedEvent(Id, OrderId, Value, PaymentDate.Value, CodeTransaction));
    }
}
