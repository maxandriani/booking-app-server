using Booking.Core.Places.ViewModels;
using MediatR;

namespace Booking.Core.Places.Commands;

public record CreatePlaceCmd(
    string Name,
    string Address
) : IRequest<PlaceResponse>
{
    public string Name { get; init; } = Name.Trim();
    public string Address { get; init; } = Address.Trim();
}