namespace Sales.Domain.Orders.Enums;

public enum OrderStatus
{
    Pending = 301,      // Aguardando confirmação de pagamento ou processamento inicial
    Confirmed = 302,    // Pagamento confirmado/sucesso na transação
    Processing = 303,   // Em separação / Preparando para envio
    Shipped = 304,      // Despachado / Com a transportadora    
    Delivered = 305,    // Entregue ao cliente final
    Cancelled = 306     // Pedido cancelado (por qualquer motivo)
}
