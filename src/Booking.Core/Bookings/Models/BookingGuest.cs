using Booking.Core.Guests.Models;

namespace Booking.Core.Bookings.Models;

public class BookingGuest
{
    public BookingGuest()
    {
    }

    public BookingGuest(Guid bookingId, Guid guestId, bool isPrincipal = false)
    {
        BookingId = bookingId;
        GuestId = guestId;
        IsPrincipal = isPrincipal;
    }

    public BookingGuest(Booking booking, Guest guest, bool isPrincipal = false)
    {
        Booking = booking;
        Guest = guest;
        IsPrincipal = isPrincipal;
    }

    public Guid BookingId { get; set; }
    public Guid GuestId { get; set; }
    public bool IsPrincipal { get; set; } = false;

    public Booking? _booking;
    public Booking? Booking
    {
        get => _booking;
        set
        {
            if (value is not null)
                BookingId = value.Id;
            _booking = value;
        }
    }
    private Guest? _guest;
    public Guest? Guest
    {
        get => _guest;
        set
        {
            if (value is not null)
                GuestId = value.Id;
            _guest = value;
        }
    }
}