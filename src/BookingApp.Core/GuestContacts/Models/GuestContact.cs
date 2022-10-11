using BookingApp.Core.Guests.Models;

namespace BookingApp.Core.GuestContacts.Models;

/// <summary>
/// Dado de contato de um Hóspede.
/// </summary>
public class GuestContact
{
    public GuestContact()
    {
    }

    public GuestContact(Guid id, Guid guestId, GuestContactTypeEnum type, string value)
    {
        Id = id;
        GuestId = guestId;
        Type = type;
        Value = value;
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid GuestId { get; set; }
    public GuestContactTypeEnum Type { get; set; } = GuestContactTypeEnum.Undefined;
    public Guest? Guest { get; set; }

    private string _value = string.Empty;
    public string Value { get => _value; set => _value = value.Trim(); }
}
