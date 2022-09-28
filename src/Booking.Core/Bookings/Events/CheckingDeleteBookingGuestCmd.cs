using MediatR;

namespace Booking.Core.Bookings.Events;

public record CheckingDeleteBookingGuestCmd(
    Guid BookingId,
    Guid GuestId
) : INotification;
