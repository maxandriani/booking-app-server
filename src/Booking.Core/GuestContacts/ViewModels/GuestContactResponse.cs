using Booking.Core.GuestContacts.Models;

namespace Booking.Core.GuestContacts.ViewModels;

public record GuestContactResponse
(
    Guid Id,
    Guid GuestId,
    GuestContactTypeEnum Type = GuestContactTypeEnum.Undefined,
    string Value = default!
)
{
    public GuestContactResponse(GuestContact contact) : this(contact.Id, contact.GuestId, contact.Type, contact.Value)
    {}
}