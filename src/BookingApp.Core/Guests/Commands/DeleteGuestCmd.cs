using MediatR;

namespace BookingApp.Core.Guests.Commands;

/// <summary>
/// Remove um registro de <see cref="BookingApp.Core.Guests.Models.Guest" />
/// </summary>
public record DeleteGuestCmd(Guid Id) : IRequest;