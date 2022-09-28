using Booking.Core.Bookings.ViewModels;
using MediatR;

namespace Booking.Core.Bookings.Commands;

public record UpdateBookingCmd(
    Guid Id,
    Guid PlaceId,
    DateTime CheckIn,
    DateTime CheckOut,
    string? Description
) : IRequest<BookingResponse>;
