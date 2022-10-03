using Booking.Core.Guests.Models;
using Booking.Core.Places.Models;

namespace Booking.Core.Bookings.Models;

public class Booking
{
    public Booking()
    {
    }

    public Booking(Guid id, Guid placeId, DateTime checkIn, DateTime checkOut, string? description = null, BookingStatusEnum? status = null)
    {
        Id = id;
        PlaceId = placeId;
        CheckIn = checkIn;
        CheckOut = checkOut;
        Description = description;
        if (status != null) Status = status.Value;
    }

    public Guid Id { get; set; }
    public Guid PlaceId { get; set; }
    public Place? Place { get; set; }
    public DateTime CheckIn { get; set; } = DateTime.UtcNow;
    public DateTime CheckOut { get; set; } = DateTime.UtcNow;
    public BookingStatusEnum Status { get; set; } = BookingStatusEnum.Pending;
    public string? Description { get; set; } = null;

    private List<BookingGuest> _guests = new();
    public IReadOnlyList<BookingGuest> Guests {
        get => _guests;
        set => _guests.Select(x =>
        {
            x.BookingId = Id;
            return x;
        });
    }

    public void AddGuest(Guest guest, bool IsPrincipal = false)
    {
        if (IsPrincipal == true)
            foreach (var g in _guests.Where(x => x.IsPrincipal == true))
                g.IsPrincipal = false;
        
        _guests.Add(new BookingGuest(this, guest, IsPrincipal));
    }
}
