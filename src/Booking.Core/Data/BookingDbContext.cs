using Microsoft.EntityFrameworkCore;
using Booking.Core.Guests.Models;

namespace Booking.Core.Data;

public class BookingDbContext : DbContext
{
    public DbSet<Guest> Guests => Set<Guest>();

    public BookingDbContext(DbContextOptions options) : base(options)
    {
    }
}