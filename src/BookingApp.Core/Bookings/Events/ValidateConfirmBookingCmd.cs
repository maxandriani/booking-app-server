using MediatR;

namespace BookingApp.Core.Bookings.Events;

public record ValidateConfirmBookingCmd(
    Guid BookingId
) : INotification;
