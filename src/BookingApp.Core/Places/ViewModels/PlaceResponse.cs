using BookingApp.Core.Places.Models;

namespace BookingApp.Core.Places.ViewModels;

public record PlaceResponse(
    Guid Id,
    string Name,
    string Address
)
{
    public PlaceResponse(Place place) : this(place.Id, place.Name, place.Address)
    { }
}