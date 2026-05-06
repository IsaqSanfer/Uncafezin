using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;

namespace Sales.Domain.Orders.ValueObjects;

public sealed class CancelReason: ValueObject
{
    public string Code { get; }
    public string Description { get; }

    // Conjunto de códigos de cancelamento pré-definidos
    private static readonly Dictionary<string, string> _predefinedReasons = new()
    {
        { "ClienteDesistiu", "Cancelamento solicitado pelo cliente" },
        { "ErroPagamento", "Erro no processamento do pagamento" },
        { "ItemSemEstoque", "Produto indisponível em estoque" },
        { "EndereçoInvalido", "Endereço de entrega incorreto" },
        { "Outro", "Motivo não especificado" }
    };

    // Construtor privado para garantir que apenas códigos pré-definidos sejam usados
    private CancelReason(string code)
    {
        if (string.IsNullOrWhiteSpace(code))
            throw new DomainException("O código do motivo de cancelamento é obrigatório.");

        if (!_predefinedReasons.ContainsKey(code))
            throw new DomainException($"Motivo de cancelamento inválido: {code}");

        Code = code;
        Description = _predefinedReasons[code];
    }

    // Factory methods para criar instâncias de CancelReason com códigos pré-definidos
    public static CancelReason ClienteDesistiu() => new CancelReason("ClienteDesistiu");
    public static CancelReason ErroPagamento() => new CancelReason("ErroPagamento");
    public static CancelReason ItemSemEstoque() => new CancelReason("ItemSemEstoque");
    public static CancelReason EndereçoInvalido() => new CancelReason("EndereçoInvalido");
    public static CancelReason Outro() => new CancelReason("Outro");

    // Igualdade estrutural
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Code;
        yield return Description;
    }

    // Sobrescreve ToString para facilitar a leitura do motivo de cancelamento (logs, relatórios, etc.)
    public override string ToString() => $"{Description}";
}