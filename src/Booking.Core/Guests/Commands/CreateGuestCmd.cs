using Booking.Core.Guests.Responses;
using MediatR;

namespace Booking.Core.Guests.Commands;

/// <summary>
/// Cria um novo registro de <see cref="Booking.Core.Guests.Models.Guest" />.
/// </summary>
public class CreateGuestCmd : IRequest<GuestResponse>
{
    public GuestCreateUpdateBody? Body { get; set; }

    public CreateGuestCmd(GuestCreateUpdateBody? body)
    {
        Body = body;
    }
}