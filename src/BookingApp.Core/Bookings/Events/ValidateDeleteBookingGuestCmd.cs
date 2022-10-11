using MediatR;

namespace BookingApp.Core.Bookings.Events;

public record ValidateDeleteBookingGuestCmd(
    Guid BookingId,
    Guid GuestId
) : INotification;
