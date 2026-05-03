using Sales.Domain.Common.Exceptions;

namespace Sales.Domain.Common.Validation;

internal static class Guard
{
    // Impede que um Guid seja vazio, o que pode ser um valor inválido para identificadores.
    public static void AgainstEmptyGuid(Guid id, string paramName, string? message = null)
    {
        if (id == Guid.Empty)
            throw new DomainException(message ?? $"{paramName} não pode ser um Guid vazio.");
    }

    // Impede que um valor seja nulo, o que pode causar erros de referência nula em tempo de execução.
    public static void AgainstNull<T>(T value, string paramName)
    {
        if (value == null)
            throw new DomainException($"{paramName} não pode ser nulo.");
    }

    // Impede que uma string seja nula ou vazia 
    public static void AgainstNullOrWhiteSpace(string value, string paramName, string? message = null)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException(message ?? $"{paramName} não pode ser nulo ou vazio.");
    }

    // Impede que um valor numérico seja negativo, o que pode ser inválido para quantidades, preços, etc.
    public static void Against<TException>(bool condition, string message) where TException : Exception
    {
        if (condition)
            throw (TException)Activator.CreateInstance(typeof(TException), message)!;
    }
}
