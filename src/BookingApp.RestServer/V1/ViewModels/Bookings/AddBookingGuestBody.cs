namespace BookingApp.RestServer.V1.ViewModels.Bookings;

public record AddBookingGuestBody(
    Guid GuestId,
    bool? IsPrimary = false
);
