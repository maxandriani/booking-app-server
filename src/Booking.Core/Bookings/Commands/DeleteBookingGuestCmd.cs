using MediatR;

namespace Booking.Core.Bookings.Commands;

public record DeleteBookingGuestCmd(
    Guid BookingId,
    Guid GuestId
) : IRequest;
