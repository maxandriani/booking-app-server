using BookingApp.Core.Commons.Exceptions;

namespace BookingApp.Core.Bookings.Exceptions;

public class BookingNotConfirmedException : BusinessException
{
    public BookingNotConfirmedException(Guid bookingId) : base(
        $"Reserva {bookingId} não foi cancelada pois não está confirmada.", null)
    {
    }
}