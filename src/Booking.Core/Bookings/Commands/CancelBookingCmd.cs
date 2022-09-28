using MediatR;

namespace Booking.Core.Bookings.Commands;

public record CancelBookingCmd(
    Guid BookingId,
    Guid GuestId
) : IRequest;