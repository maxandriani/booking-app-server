using MediatR;

namespace BookingApp.Core.Bookings.Commands;

public record DeleteBookingGuestCmd(
    Guid BookingId,
    Guid GuestId
) : IRequest;
