using Sales.Domain.Common.Base;
using Sales.Domain.Common.Exceptions;
using Sales.Domain.Common.Validation;
using System.Text.RegularExpressions;

namespace Sales.Domain.Customers.ValueObjects;

public sealed class Email : ValueObject
{
    public string Address { get; }

    private static readonly Regex _regex = new(@"^[\w\.-]+@[\w\.-]+\.\w{2,}$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public Email(string address)
    {
        Guard.AgainstNullOrWhiteSpace(address, nameof(address), "O email é obrigatório.");
        Guard.Against<DomainException>(!_regex.IsMatch(address), "Email inválido.");

        Address = address.Trim().ToLowerInvariant();
    }

    public override string ToString()
    {
        return Address;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Address;
    }

    private static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }
}
