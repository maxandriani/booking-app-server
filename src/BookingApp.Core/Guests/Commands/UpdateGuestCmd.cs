using BookingApp.Core.Guests.Models;
using BookingApp.Core.Guests.ViewModels;
using MediatR;

namespace BookingApp.Core.Guests.Commands;

/// <summary>
/// Atualiza um registro de <see cref="BookingApp.Core.Guests.Models.Guest" />
/// </summary>
public record UpdateGuestCmd(
    Guid Id,
    string Name
) : IRequest<GuestWithContactsResponse>
{
    public string Name { get; init; } = Name.Trim();
}
