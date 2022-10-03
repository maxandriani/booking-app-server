using MediatR;

namespace BookingApp.Core.Bookings.Events;

public record ValidateCancelBookingCmd(
    Guid BookingId
) : INotification;