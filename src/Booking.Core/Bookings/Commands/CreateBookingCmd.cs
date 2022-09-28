using Booking.Core.Bookings.ViewModels;
using MediatR;

namespace Booking.Core.Bookings.Commands;

public record CreateBookingCmd(
    Guid PlaceId,
    DateTime CheckIn,
    DateTime CheckOut,
    string? Description
) : IRequest<BookingResponse>
{
    public string? Description { get; init; } = Description?.Trim() ?? null;
}