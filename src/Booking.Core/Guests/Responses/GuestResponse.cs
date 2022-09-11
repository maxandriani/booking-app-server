using Booking.Core.Guests.Models;

namespace Booking.Core.Guests.Responses;

public class GuestResponse
{
    public GuestResponse()
    {
    }

    public GuestResponse(Guest guest)
    {
        Id = guest.Id;
        Name = guest.Name;
    }

    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
}