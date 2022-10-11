using BookingApp.Core.Places.ViewModels;

namespace BookingApp.Core.Bookings.ViewModels;

public record BookingAvailabilityResponse(
    BookingAvailabilityStatusEnum Status,
    PlaceResponse Place
);
