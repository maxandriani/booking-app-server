using Microsoft.EntityFrameworkCore;
using BookingApp.Core.Guests.Models;
using BookingApp.Core.GuestContacts.Models;
using BookingApp.Core.Places.Models;
using BookingApp.Core.Bookings.Models;

namespace BookingApp.Core.Data;

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

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var place = builder.Entity<Place>();
        place.HasMany(p => p.Bookings)
            .WithOne(p => p.Place)
            .HasForeignKey(p => p.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);

        var guest = builder.Entity<Guest>();
        guest.HasMany(p => p.Contacts)
            .WithOne(p => p.Guest)
            .HasForeignKey(p => p.GuestId)
            .OnDelete(DeleteBehavior.Cascade);
        guest.HasMany(p => p.Bookings)
            .WithOne(p => p.Guest)
            .HasForeignKey(q => q.GuestId)
            .OnDelete(DeleteBehavior.Cascade);

        var guestContact = builder.Entity<GuestContact>();
        guestContact.HasOne(q => q.Guest)
            .WithMany(q => q.Contacts)
            .HasForeignKey(q => q.GuestId)
            .OnDelete(DeleteBehavior.Cascade);
        guestContact.HasIndex(q => q.Type, "idx_guestcontact_type_search");

        var booking = builder.Entity<Bookings.Models.Booking>();
        booking.HasOne(q => q.Place)
            .WithMany(q => q.Bookings)
            .HasForeignKey(q => q.PlaceId)
            .OnDelete(DeleteBehavior.Cascade);
        booking.HasMany(q => q.Guests)
            .WithOne(q => q.Booking)
            .HasForeignKey(q => q.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
        booking.HasIndex(q => new { q.CheckIn }, "idx_booking_checkin_search");
        booking.HasIndex(q => new { q.CheckOut }, "idx_booking_checkout_search");

        var bookingGuest = builder.Entity<BookingGuest>();
        bookingGuest.HasKey(x => new { x.BookingId, x.GuestId });
        bookingGuest.HasOne(q => q.Booking)
            .WithMany(q => q.Guests)
            .HasForeignKey(q => q.BookingId)
            .OnDelete(DeleteBehavior.Cascade);
        bookingGuest.HasOne(q => q.Guest)
            .WithMany(q => q.Bookings)
            .HasForeignKey(q => q.GuestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}