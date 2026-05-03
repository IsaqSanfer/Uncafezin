namespace Sales.Domain.Common.Enums;

public enum PaymentStatus
{
    Pending = 401,      // Aguardando pagamento ou processamento inicial
    Approved = 402,     // Pagamento concluído com sucesso
    Refused = 403,      // Falha no pagamento (ex: cartão recusado)
    Refunded = 404,     // Pagamento reembolsado ao cliente
    Cancelled = 405     // Pagamento cancelado (por qualquer motivo)
}
