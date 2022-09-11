using Booking.Core.Guests.Responses;
using MediatR;

namespace Booking.Core.Guests.Commands;

/// <summary>
/// Atualiza um registro de <see cref="Booking.Core.Guests.Models.Guest" />
/// </summary>
public class UpdateGuestCmd : IRequest<GuestResponse>
{
    public Guid Id { get; set; }
    public GuestCreateUpdateBody? Body { get; set; }

    public UpdateGuestCmd(Guid id, GuestCreateUpdateBody? body)
    {
        Id = id;
        Body = body;
    }
}