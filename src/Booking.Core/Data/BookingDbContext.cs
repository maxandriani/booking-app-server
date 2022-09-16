using Microsoft.EntityFrameworkCore;
using Booking.Core.Guests.Models;
using Booking.Core.GuestContacts.Models;
using Booking.Core.Places.Models;

namespace Booking.Core.Data;

public class BookingDbContext : DbContext
{
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<GuestContact> GuestContacts => Set<GuestContact>();
    public DbSet<Place> Places => Set<Place>();

    public BookingDbContext(DbContextOptions options) : base(options)
    {
    }
}