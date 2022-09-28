using MediatR;

namespace Booking.Core.Bookings.Events;

public record CheckingCancelBookingCmd(
    Guid BookingId
) : INotification;