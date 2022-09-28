using Booking.Core.Places.ViewModels;

namespace Booking.Core.Bookings.ViewModels;

public record BookingAvailabilityResponse(
    BookingAvailabilityStatusEnum Status,
    PlaceResponse Place
);
