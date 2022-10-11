using BookingApp.Core.GuestContacts.Models;
using BookingApp.Core.Guests.ViewModels;
using MediatR;

namespace BookingApp.Core.Guests.Commands;

/// <summary>
/// Cria um novo registro de <see cref="BookingApp.Core.Guests.Models.Guest" />.
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