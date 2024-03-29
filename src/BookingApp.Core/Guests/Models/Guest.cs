using System.Collections.ObjectModel;
using BookingApp.Core.GuestContacts.Models;
using BookingApp.Core.Bookings.Models;

namespace BookingApp.Core.Guests.Models;

/// <summary>
/// Aquele que aluga um espaço.
/// </summary>
public class Guest
{
    public Guest()
    {
    }

    public Guest(Guid id, string name, ReadOnlyCollection<GuestContact>? contacts = null)
    {
        Id = id;
        Name = name.Trim();
        if (contacts != null)
            Contacts = contacts;
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    private string _name = string.Empty;
    public string Name { get => _name; set => _name = value.Trim(); }

    private List<GuestContact> _contacts = new();
    public IReadOnlyList<GuestContact> Contacts
    {
        get => _contacts;
        set => _contacts = value.Select(x => { x.GuestId = Id; return x; }).ToList();
    }

    public Guest AddContact(GuestContact contact)
    {
        contact.GuestId = Id;
        _contacts.Add(contact);
        return this;
    }

    public List<BookingGuest> Bookings { get; set; } = new();
}