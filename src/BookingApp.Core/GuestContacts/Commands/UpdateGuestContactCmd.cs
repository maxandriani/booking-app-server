using BookingApp.Core.GuestContacts.Models;
using BookingApp.Core.GuestContacts.ViewModels;
using MediatR;

namespace BookingApp.Core.GuestContacts.Commands;

/// <summary>
/// Atualiza os dados de contato de um hóspede.
/// </summary>
public record UpdateGuestContactCmd(
    Guid Id,
    Guid GuestId,
    GuestContactTypeEnum Type,
    string Value) : IRequest<GuestContactResponse>
{
    public string Value { get; init; } = Value.Trim();
}
