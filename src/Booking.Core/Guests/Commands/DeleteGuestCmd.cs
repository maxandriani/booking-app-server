using MediatR;

namespace Booking.Core.Guests.Commands;

/// <summary>
/// Remove um registro de <see cref="Booking.Core.Guests.Models.Guest" />
/// </summary>
public record DeleteGuestCmd(Guid Id) : IRequest;