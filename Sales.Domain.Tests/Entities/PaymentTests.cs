using FluentAssertions;
using Sales.Domain.Common.Base;
using Sales.Domain.Common.Enums;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Entities;
using Sales.Domain.Events;
using Xunit;

namespace Sales.Domain.Tests.Entities;

public class PaymentTests
{
    [Fact(DisplayName = "Criar um pagamento válido com status pendente")]
    public void CreateValidPayment_ShouldReturnPayment()
    {
        var orderId = Guid.NewGuid();
        var method = PaymentMethod.CreditCard;
        var value = 100.00m;

        var payment = new Payment(orderId, method, value);

        payment.OrderId.Should().Be(orderId);
        payment.PaymentMethod.Should().Be(method);
        payment.Value.Should().Be(value);
        payment.PaymentStatus.Should().Be(PaymentStatus.Pending);
        payment.PaymentDate.Should().BeNull();
        payment.CodeTransaction.Should().BeNull();
    }

    [Fact(DisplayName = "Não deve criar um pagamento com dados inválidos")]
    public void CreateInvalidPayment_ShouldThrowException()
    {
        var orderId = Guid.NewGuid();

        Action act = () => new Payment(orderId, PaymentMethod.Pix, 0);

        act.Should().Throw<DomainException>().WithMessage("O valor do pagamento deve ser maior que zero");
    }

    [Fact(DisplayName = "Não deve definir código de transação nulo ou vazio")]
    public void CreatePaymentNullOrEmptyCodeTransaction_ShouldThrowException()
    {
        var orderId = Guid.NewGuid();
        var payment = new Payment(orderId, PaymentMethod.Pix, 100.00m);

        Action actNull = () => payment.SetTransactionCode(null);
        Action actEmpty = () => payment.SetTransactionCode("");

        actNull.Should().Throw<DomainException>().WithMessage("O código de transação é obrigatório.");
        actEmpty.Should().Throw<DomainException>().WithMessage("O código de transação é obrigatório.");
    }

    [Fact(DisplayName = "Não deve definir código de transação válido")]
    public void CreatePaymentValidCodeTransaction_ShouldSetCode()
    {
        var orderId = Guid.NewGuid();
        var payment = new Payment(orderId, PaymentMethod.CreditCard, 100.00m);
        var code = "TXN-12345";

        payment.SetTransactionCode(code);

        payment.CodeTransaction.Should().Be(code);
        payment.UpdateDate.Should().NotBeNull();
    }

    [Fact(DisplayName = "Não deve redefinir o código de transação já definido")]
    public void CreatePaymentAlreadySetCodeTransaction_ShouldThrowException()
    {
        var orderId = Guid.NewGuid();
        var payment = new Payment(orderId, PaymentMethod.CreditCard, 100.00m);

        payment.SetTransactionCode("TXN-0001");

        Action act = () => payment.SetTransactionCode("TXN-67890");

        act.Should().Throw<DomainException>().WithMessage("O código de transação já foi definido.");
    }

    [Fact(DisplayName = "Gerar código de transação local automaticamente")]
    public void CreatePaymentGenerateInternalCode_ShouldSetLocalCode()
    {
        var orderId = Guid.NewGuid();
        var payment = new Payment(orderId, PaymentMethod.Pix, 100.00m);

        payment.GenerateInternalTransactionCode();

        payment.CodeTransaction.Should().StartWith("LOCAL-");
        payment.CodeTransaction.Should().HaveLength(14);    // "LOCAL-" + 8 caracteres alfanuméricos
        payment.UpdateDate.Should().NotBeNull();
    }

    [Fact(DisplayName = "Confirmar pagamento pendente com código válido e gerar evento completo")]
    public void ConfirmPayment_ShouldSetPaymentStatusToConfirmed()
    {
        var payment = new Payment(Guid.NewGuid(), PaymentMethod.CreditCard, 300m);

        payment.GenerateInternalTransactionCode();
        payment.ConfirmPayment();

        payment.PaymentStatus.Should().Be(PaymentStatus.Approved);
        payment.PaymentDate.Should().NotBeNull();
        payment.UpdateDate.Should().NotBeNull();

        var events = payment.DomainEvents.OfType<PaymentApprovedEvent>().FirstOrDefault();
        events.Should().NotBeNull();
        events!.PaymentId.Should().Be(payment.Id);
        events.OrderId.Should().Be(payment.OrderId);
        events.Value.Should().Be(payment.Value);
        events.CodeTransaction.Should().Be(payment.CodeTransaction);
        events.PaymentDate.Should().Be(payment.PaymentDate);
    }

    [Fact(DisplayName = "Não deve confirmar pagamento sem código de transação")]
    public void ConfirmPaymentWithoutTransactionCode_ShouldThrowException()
    {
        var payment = new Payment(Guid.NewGuid(), PaymentMethod.Pix, 100m);

        Action act = () => payment.ConfirmPayment();

        act.Should().Throw<DomainException>().WithMessage("O código de transação é obrigatório para confirmar o pagamento.");
    }

    [Fact(DisplayName = "Não deve confirmar pagamentos pendentes")]
    public void ConfirmPendingPayment_ShouldThrowException()
    {
        var payment = new Payment(Guid.NewGuid(), PaymentMethod.Pix, 100m);
        payment.GenerateInternalTransactionCode();
        payment.ConfirmPayment();

        Action act = () => payment.ConfirmPayment();

        act.Should().Throw<DomainException>().WithMessage("O pagamento já foi confirmado.");
    }

    [Fact(DisplayName = "Deve recusar pagamento pendente e gerar evento de rejeição com dados corretos")]
    public void RejectPayment_ShouldSetPaymentStatusToRejected()
    {
        var payment = new Payment(Guid.NewGuid(), PaymentMethod.CreditCard, 120m);

        payment.RejectPayment();

        payment.PaymentStatus.Should().Be(PaymentStatus.Refused);
        payment.PaymentDate.Should().NotBeNull();
        payment.UpdateDate.Should().NotBeNull();

        var events = payment.DomainEvents.OfType<PaymentRejectedEvent>().FirstOrDefault();
        events.Should().NotBeNull();
        events!.PaymentId.Should().Be(payment.Id);
        events.OrderId.Should().Be(payment.OrderId);
        events.Value.Should().Be(payment.Value);
        events.CodeTransaction.Should().Be(payment.CodeTransaction);
        events.PaymentDate.Should().Be(payment.PaymentDate);
    }

    [Fact(DisplayName = "Não deve recusar pagamentos que não estão pendentes")]
    public void RejectNonPendingPayment_ShouldThrowException()
    {
        var payment = new Payment(Guid.NewGuid(), PaymentMethod.Pix, 120m);
        
        payment.GenerateInternalTransactionCode();
        payment.ConfirmPayment();

        Action act = () => payment.RejectPayment();

        act.Should().Throw<DomainException>().WithMessage("Apenas pagamentos pendentes podem ser recusados");
    }

}