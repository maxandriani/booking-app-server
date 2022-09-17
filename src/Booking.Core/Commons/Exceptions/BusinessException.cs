namespace Booking.Core.Commons.Exceptions;

public abstract class BusinessException : Exception
{
    public BusinessException(string? message, Exception? innerException = null) : base(message, innerException)
    {
    }
}