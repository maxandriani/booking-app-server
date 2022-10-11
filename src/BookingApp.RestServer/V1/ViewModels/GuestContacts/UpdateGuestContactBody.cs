using BookingApp.Core.GuestContacts.Models;

namespace BookingApp.RestServer.V1.ViewModels.GuestContacts;

public class UpdateGuestContactBody
{
    public GuestContactTypeEnum Type { get; set; }
    public string Value { get; set; } = string.Empty;
}