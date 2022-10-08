using BookingApp.Core.Commons.Exceptions;

namespace BookingApp.Core.Bookings.Exceptions;

public class BookingReadOnlyStateException : BusinessException
{
    public BookingReadOnlyStateException(Guid bookingId) :
        base($"Reserva {bookingId} não pode ser alterada após confirmação.", null)
    {
    }
}