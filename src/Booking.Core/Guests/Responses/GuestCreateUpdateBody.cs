using Booking.Core.Guests.Models;

namespace Booking.Core.Guests.Responses;

public class GuestCreateUpdateBody
{
    public string Name { get; set; } = string.Empty;

    public void MapTo(Guest guest)
    {
        guest.Name = Name.Trim();
    }
}