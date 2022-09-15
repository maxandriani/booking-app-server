using Booking.Core.GuestContacts.Models;
using Booking.Core.Guests.ViewModels;
using MediatR;

namespace Booking.Core.Guests.Commands;

/// <summary>
/// Cria um novo registro de <see cref="Booking.Core.Guests.Models.Guest" />.
/// </summary>
public record CreateGuestWithContactsCmd
(
    string Name,
    List<CreateGuestContact>? Contacts = null
) : IRequest<GuestWithContactsResponse>
{
    public string Name { get; init; } = Name.Trim();
}

public record CreateGuestContact(GuestContactTypeEnum Type, string Value)
{
    public string Value { get; init; } = Value.Trim();
}