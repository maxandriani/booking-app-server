using MediatR;

namespace Booking.Core.Guests.Commands;

/// <summary>
/// Remove um registro de <see cref="Booking.Core.Guests.Models.Guest" />
/// </summary>
public class DeleteGuestCmd : IRequest
{
    public Guid Id { get; set; }

    public DeleteGuestCmd(Guid id)
    {
        Id = id;
    }
}