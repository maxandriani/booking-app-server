using Booking.Core.GuestContacts.ViewModels;
using Booking.Core.Guests.Models;

namespace Booking.Core.Guests.ViewModels;

public record GuestWithContactsResponse(
    Guid Id,
    string Name,
    List<GuestContactResponse> Contacts = default!
)
{
    public GuestWithContactsResponse(Guest guest) : this(
        guest.Id,
        guest.Name,
        guest.Contacts.Select(x => new GuestContactResponse(x)).ToList())
    {
    }
}