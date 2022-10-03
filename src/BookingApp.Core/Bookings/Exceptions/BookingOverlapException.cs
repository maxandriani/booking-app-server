using BookingApp.Core.Commons.Exceptions;

namespace BookingApp.Core.Bookings.Exceptions;

public class BookingOverlapException : BusinessException
{
    public BookingOverlapException(DateTime checkIn, DateTime checkOut, string placeName) : base($"Já existe uma reserva para o período de {checkIn} até {checkOut} no imóvel {placeName}.", null)
    { }
}