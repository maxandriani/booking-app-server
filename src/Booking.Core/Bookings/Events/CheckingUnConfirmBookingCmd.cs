using MediatR;

namespace Booking.Core.Bookings.Events;

public record CheckingUnConfirmBookingCmd(
    Guid BookingId
) : INotification;
