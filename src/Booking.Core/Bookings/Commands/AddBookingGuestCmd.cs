using MediatR;

namespace Booking.Core.Bookings.Commands;

public record AddBookingGuestCmd(
    Guid BookingId,
    Guid GuestId
) : IRequest;