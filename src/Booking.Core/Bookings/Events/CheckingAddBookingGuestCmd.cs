using MediatR;

namespace Booking.Core.Bookings.Events;

public record CheckingAddBookingGuestCmd(
    Guid BookingId,
    Guid GuestId
) : INotification;