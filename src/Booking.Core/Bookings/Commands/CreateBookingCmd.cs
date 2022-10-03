using Booking.Core.Bookings.ViewModels;
using MediatR;

namespace Booking.Core.Bookings.Commands;

public record CreateBookingCmd(
    Guid PlaceId,
    DateTime CheckIn,
    DateTime CheckOut,
    string? Description = null
) : IRequest<BookingResponse>
{
    public string? Description { get; init; } = Description?.Trim() ?? null;
    public DateTime CheckIn { get; init; } = CheckIn.Date;
    public DateTime CheckOut { get; init; } = CheckOut.Date;
}