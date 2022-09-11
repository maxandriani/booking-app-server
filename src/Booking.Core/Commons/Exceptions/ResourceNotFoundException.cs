namespace Booking.Core.Commons.Exceptions;

public class ResourceNotFoundException : Exception
{
    public ResourceNotFoundException(string? resourceName, Exception? innerException) : base($"{resourceName} não foi encontrado.", innerException)
    {
    }

    public ResourceNotFoundException(string? resourceName) : this(resourceName, null)
    {
    }
}