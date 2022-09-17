using Booking.Core.Commons.Exceptions;

namespace Booking.Core.Bookings.Exceptions;

public class BookingOverlapException : BusinessException
{
    public BookingOverlapException(DateTime checkIn, DateTime checkOut, string placeName) : base($"Já existe uma reserva para o período de {checkIn} até {checkOut} no imóvel {placeName}.", null)
    {}
}