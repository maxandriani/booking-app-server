using MediatR;

namespace Booking.Core.Bookings.Events;

public record CheckingConfirmBookingCmd(
    Guid BookingId
) : INotification;
