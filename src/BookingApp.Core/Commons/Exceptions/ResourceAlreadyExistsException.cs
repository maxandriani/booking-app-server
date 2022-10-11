namespace BookingApp.Core.Commons.Exceptions;

public class ResourceAlreadyExistsException : Exception
{
    public ResourceAlreadyExistsException(string chave, string resourceName, Exception? innerException = null) : base($"Já existe um registro de {resourceName} com o valor '{chave}'.", innerException)
    {
    }
}