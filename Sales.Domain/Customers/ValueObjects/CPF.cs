using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;

namespace Sales.Domain.Customers.ValueObjects;

public sealed class CPF : ValueObject
{
    public string Number { get; }

    public CPF(string number)
    {
        Guard.AgainstNullOrWhiteSpace(number, nameof(number), "O CPF é obrigatório.");

        var digits = new string(number.Where(char.IsDigit).ToArray());

        Guard.Against<DomainException>(digits.Length != 11, "O CPF deve conter 11 dígitos.");
        Guard.Against<DomainException>(!IsValidCPF(number), "O CPF é inválido.");
        
        Number = digits;
    }

    public override string ToString()
    {
        return Convert.ToUInt64(Number).ToString(@"000\.000\.000\-00");
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Number;
    }

    private static bool IsValidCPF(string cpf)
    {
        var digits = new string(cpf.Where(char.IsDigit).ToArray());
        if (digits.Length != 11)
            return false;

        // Verificar se todos os dígitos são iguais (ex: 111.111.111-11)
        if (digits.Distinct().Count() == 1)
            return false;

        // Cálculo dos dígitos verificadores
        var numbers = digits.Select(d => int.Parse(d.ToString())).ToArray();
        var sum1 = 0;
        for (int i = 0; i < 9; i++)
            sum1 += numbers[i] * (10 - i);

        var remainder1 = sum1 % 11;
        var digit1 = remainder1 < 2 ? 0 : 11 - remainder1;
        var sum2 = 0;
        for (int i = 0; i < 10; i++)
            sum2 += numbers[i] * (11 - i);

        var remainder2 = sum2 % 11;
        var digit2 = remainder2 < 2 ? 0 : 11 - remainder2;
        return digit1 == numbers[9] && digit2 == numbers[10];
    }
}
