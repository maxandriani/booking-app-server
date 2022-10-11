namespace BookingApp.Core.Commons.Exceptions;

public class ResourceNotFoundException : Exception
{
    public ResourceNotFoundException(string? resourceName, Exception? innerException = null) : base($"{resourceName} não foi encontrado.", innerException)
    {
    }
}