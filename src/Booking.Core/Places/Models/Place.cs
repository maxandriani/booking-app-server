namespace Booking.Core.Places.Models;

public class Place
{
    public Place()
    {
    }

    public Place(Guid id, string name, string address)
    {
        Id = id;
        Name = name;
        Address = address;
    }

    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;

    public List<Bookings.Models.Booking> Bookings { get; set; } = new();
}