using Microsoft.EntityFrameworkCore;
using Booking.Core.Guests.Models;
using Booking.Core.GuestContacts.Models;
using Booking.Core.Places.Models;
using Booking.Core.Bookings.Models;

namespace Booking.Core.Data;

public class BookingDbContext : DbContext
{
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<GuestContact> GuestContacts => Set<GuestContact>();
    public DbSet<Place> Places => Set<Place>();
    public DbSet<Bookings.Models.Booking> Bookings => Set<Bookings.Models.Booking>();
    public DbSet<BookingGuest> BookingGuests => Set<BookingGuest>();

    public BookingDbContext(DbContextOptions options) : base(options)
    {
    }
}