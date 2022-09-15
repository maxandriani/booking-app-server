using Booking.Core.GuestContacts.Models;

namespace Booking.RestServer.V1.ViewModels.GuestContacts;

public class UpdateGuestContactBody
{
    public GuestContactTypeEnum Type { get; set; }
    public string Value { get; set; } = string.Empty;
}