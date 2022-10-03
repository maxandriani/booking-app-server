using BookingApp.Core.GuestContacts.ViewModels;

namespace BookingApp.Core.Bookings.ViewModels;

public record BookingGuestResponse(
    Guid Id,
    string Name,
    bool IsPrincipal,
    List<GuestContactResponse> Contacts
);