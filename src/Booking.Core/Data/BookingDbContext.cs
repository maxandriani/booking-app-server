using Microsoft.EntityFrameworkCore;
using Booking.Core.Guests.Models;
using Booking.Core.GuestContacts.Models;

namespace Booking.Core.Data;

public class BookingDbContext : DbContext
{
    public DbSet<Guest> Guests => Set<Guest>();
    public DbSet<GuestContact> GuestContacts => Set<GuestContact>();

    public BookingDbContext(DbContextOptions options) : base(options)
    {
    }
}