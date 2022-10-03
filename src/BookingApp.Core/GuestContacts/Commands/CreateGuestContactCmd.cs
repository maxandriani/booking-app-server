using BookingApp.Core.GuestContacts.Models;
using BookingApp.Core.GuestContacts.ViewModels;
using MediatR;

namespace BookingApp.Core.GuestContacts.Commands;

/// <summary>
/// Cria um contato com base em um hóspede.
/// </summary>
public record CreateGuestContactCmd(
    Guid GuestId,
    GuestContactTypeEnum Type,
    string Value) : IRequest<GuestContactResponse>
{
    public string Value { get; init; } = Value.Trim();
}
