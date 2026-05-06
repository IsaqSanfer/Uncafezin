namespace Sales.Domain.Orders.Enums;

public enum PaymentStatus
{
    Pending = 401,      // Aguardando pagamento ou processamento inicial
    Approved = 402,     // Pagamento concluído com sucesso
    Refused = 403,      // Falha no pagamento (ex: cartão recusado)
    Reversed = 404,     // Pagamento estornado ao cliente
    Cancelled = 405     // Pagamento cancelado (por qualquer motivo)
}
