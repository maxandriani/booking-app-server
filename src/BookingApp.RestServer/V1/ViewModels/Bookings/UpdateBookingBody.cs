namespace BookingApp.RestServer.V1.ViewModels.Bookings;

public record UpdateBookingBody(
    Guid PlaceId,
    DateTime CheckIn,
    DateTime CheckOut,
    string? Description = null
);
