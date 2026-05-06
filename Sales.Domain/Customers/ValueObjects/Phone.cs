using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Customers.ValueObjects;

public sealed class Phone : ValueObject
{
    public string Number { get; }

    public Phone(string number)
    {
        Guard.AgainstNullOrWhiteSpace(number, nameof(number), "O telefone é obrigatório.");

        var digits = new string(number.Where(char.IsDigit).ToArray());

        Guard.Against<DomainException>(digits.Length < 10 || digits.Length > 11, "O telefone deve conter 10 ou 11 dígitos (celular).");

        Number = digits;
    }

    public override string ToString()
    {
        // (99) 99999-9999 ou (99) 9999-9999
        if (Number.Length == 11)
            return Convert.ToUInt64(Number).ToString(@"\(00\) 00000\-0000");

        return Convert.ToUInt64(Number).ToString(@"\(00\) 0000\-0000");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }
}
