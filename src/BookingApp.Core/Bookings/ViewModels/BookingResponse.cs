using BookingApp.Core.Bookings.Models;
using BookingApp.Core.GuestContacts.ViewModels;
using BookingApp.Core.Guests.ViewModels;
using BookingApp.Core.Places.ViewModels;

namespace BookingApp.Core.Bookings.ViewModels;

public record BookingResponse(
    Guid Id,
    Guid PlaceId,
    DateTime CheckIn,
    DateTime CheckOut,
    BookingStatusEnum Status,
    string? Description,
    PlaceResponse Place,
    List<BookingGuestResponse> Guests
)
{
    public BookingResponse(Bookings.Models.Booking booking) : this(
        booking.Id,
        booking.PlaceId,
        booking.CheckIn,
        booking.CheckOut,
        booking.Status,
        booking.Description,
        new PlaceResponse(booking.Place ?? throw new ArgumentNullException($"{nameof(booking.Place)} não pode ser nulo.")),
        booking.Guests.Select(x => new BookingGuestResponse(
            x.GuestId,
            x.Guest?.Name ?? throw new ArgumentNullException($"{nameof(x.Guest)} não pode ser nulo."),
            x.IsPrincipal,
            x.Guest.Contacts.Select(x => new GuestContactResponse(x)).ToList()
        )).ToList())
    { }
}
