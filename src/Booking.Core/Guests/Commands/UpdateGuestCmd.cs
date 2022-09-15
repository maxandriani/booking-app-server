using Booking.Core.Guests.Models;
using Booking.Core.Guests.ViewModels;
using MediatR;

namespace Booking.Core.Guests.Commands;

/// <summary>
/// Atualiza um registro de <see cref="Booking.Core.Guests.Models.Guest" />
/// </summary>
public record UpdateGuestCmd(
    Guid Id,
    string Name
) : IRequest<GuestWithContactsResponse>
{
    public string Name { get; init; } = Name.Trim();
}
