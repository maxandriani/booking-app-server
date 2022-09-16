using Booking.Core.Places.ViewModels;
using MediatR;

namespace Booking.Core.Places.Commands;

public record UpdatePlaceCmd(
    Guid Id,
    string Name,
    string Address
) : IRequest<PlaceResponse>
{
    public string Name = Name.Trim();
    public string Address = Address.Trim();
}
