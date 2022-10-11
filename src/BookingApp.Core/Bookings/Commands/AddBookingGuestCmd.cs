using MediatR;

namespace BookingApp.Core.Bookings.Commands;

public record AddBookingGuestCmd(
    Guid BookingId,
    Guid GuestId,
    bool? IsPrimary = false
) : IRequest;