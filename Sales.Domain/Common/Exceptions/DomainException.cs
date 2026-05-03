namespace Sales.Domain.Common.Exceptions;

public class DomainException : Exception
{
    public DomainException(string message) : base(message)
    {
    }

    // Validações de pré-condição
    public static void When(bool hasError, string errorMessage)
    {
        if (hasError)
            throw new DomainException(errorMessage);
    }
}
