using MediatR;

namespace BookingApp.Core.Bookings.Events;

public record ValidateAddBookingGuestCmd(
    Guid BookingId,
    Guid GuestId
) : INotification;