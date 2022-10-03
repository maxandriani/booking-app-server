namespace Booking.Core.Bookings.Commands;

public record SetPrimaryBookingGuestCmd(
    Guid BookingId,
    Guid GuestId
);
