using MediatR;

namespace BookingApp.Core.Bookings.Events;

public record ValidateUnConfirmBookingCmd(
    Guid BookingId
) : INotification;
