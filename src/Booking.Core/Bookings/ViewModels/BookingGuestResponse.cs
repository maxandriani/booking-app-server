using Booking.Core.GuestContacts.ViewModels;

namespace Booking.Core.Bookings.ViewModels;

public record BookingGuestResponse(
    Guid Id,
    string Name,
    bool IsPrincipal,
    List<GuestContactResponse> Contacts
);