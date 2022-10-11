using BookingApp.Core.Bookings.Commands;
using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.Guests.Models;
using BookingApp.Core.Places.Models;
using BookingApp.Core.Tests.Commons;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace BookingApp.Core.Tests.Bookings;

public class DeleteBookingGuestCmdHandlerTest : TestBase
{
    [Fact]
    public async Task Should_thorw_ValidationException_when_booking_id_is_empty()
    {
       var cmd = new DeleteBookingGuestCmd(Guid.Empty, Guid.NewGuid());
        await Should.ThrowAsync<ValidationException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_throw_ValidationException_when_guest_id_is_empty()
    {
        var cmd = new DeleteBookingGuestCmd(Guid.NewGuid(), Guid.Empty);
        await Should.ThrowAsync<ValidationException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_throw_ResourceNotFoundException_when_booking_does_not_exists()
    {
        var cmd = new DeleteBookingGuestCmd(Guid.NewGuid(), Guid.NewGuid());
        await Should.ThrowAsync<ResourceNotFoundException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_throw_ResourceNotFoundException_when_booking_guest_does_not_exists()
    {
        var place = new Place(Guid.NewGuid(), "BG Place", "24th, BG Street");
        var booking = new Booking(Guid.NewGuid(), place.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), "BG Booking", BookingStatusEnum.Confirmed);
        _dbContext.Add(place);
        _dbContext.Add(booking);
        await _dbContext.SaveChangesAsync();

        var cmd = new DeleteBookingGuestCmd(booking.Id, Guid.NewGuid());
        await Should.ThrowAsync<ResourceNotFoundException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_remove_a_booking_guest_relation()
    {
        var place = new Place(Guid.NewGuid(), "BG Place", "24th, BG Street");
        var booking = new Booking(Guid.NewGuid(), place.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), "BG Booking", BookingStatusEnum.Confirmed);
        var guest = new Guest(Guid.NewGuid(), "Guest Name");
        var bg = new BookingGuest(booking.Id, guest.Id, true);
        _dbContext.Add(place);
        _dbContext.Add(booking);
        _dbContext.Add(guest);
        _dbContext.Add(bg);
        await _dbContext.SaveChangesAsync();

        var cmd = new DeleteBookingGuestCmd(booking.Id, guest.Id);
        await _mediator.Send(cmd);

        var proof = await _dbContext.BookingGuests.AnyAsync(q => q.BookingId == booking.Id && q.GuestId == q.GuestId);
        var guestProof = await _dbContext.Guests.AnyAsync(q => q.Id == guest.Id);

        proof.ShouldBeFalse();
        guestProof.ShouldBeTrue();
    }

    [Fact]
    public async Task Should_grand_other_booking_guest_relation_as_primary_when_remove_a_primary_relation()
    {
        var place = new Place(Guid.NewGuid(), "BG Place", "24th, BG Street");
        var booking = new Booking(Guid.NewGuid(), place.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), "BG Booking", BookingStatusEnum.Confirmed);
        var guest1 = new Guest(Guid.NewGuid(), "Guest Name");
        var bg1 = new BookingGuest(booking.Id, guest1.Id, true);
        var guest2 = new Guest(Guid.NewGuid(), "Guest 2");
        var bg2 = new BookingGuest(booking.Id, guest2.Id);
        var guest3 = new Guest(Guid.NewGuid(), "Guest 3");
        var bg3 = new BookingGuest(booking.Id, guest3.Id);
        _dbContext.Add(place);
        _dbContext.Add(booking);
        _dbContext.Add(guest1);
        _dbContext.Add(guest2);
        _dbContext.Add(guest3);
        _dbContext.Add(bg1);
        _dbContext.Add(bg2);
        _dbContext.Add(bg3);
        await _dbContext.SaveChangesAsync();

        var cmd = new DeleteBookingGuestCmd(booking.Id, guest1.Id);
        await _mediator.Send(cmd);

        var proofPrimary = await _dbContext.BookingGuests.FirstAsync(q => q.BookingId == booking.Id && q.GuestId == guest2.Id);
        var proofNotPrimary = await _dbContext.BookingGuests.FirstAsync(q => q.BookingId == booking.Id && q.GuestId == guest3.Id);

        proofPrimary.IsPrincipal.ShouldBeTrue();
        proofNotPrimary.IsPrincipal.ShouldBeFalse();
    }

    [Fact]
    public async Task Should_not_grant_other_relation_if_removing_a_nom_primary_booking_grant_relation()
    {
        var place = new Place(Guid.NewGuid(), "BG Place", "24th, BG Street");
        var booking = new Booking(Guid.NewGuid(), place.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), "BG Booking", BookingStatusEnum.Confirmed);
        var guest1 = new Guest(Guid.NewGuid(), "Guest Name");
        var bg1 = new BookingGuest(booking.Id, guest1.Id);
        var guest2 = new Guest(Guid.NewGuid(), "Guest 2");
        var bg2 = new BookingGuest(booking.Id, guest2.Id);
        var guest3 = new Guest(Guid.NewGuid(), "Guest 3");
        var bg3 = new BookingGuest(booking.Id, guest3.Id, true);
        _dbContext.Add(place);
        _dbContext.Add(booking);
        _dbContext.Add(guest1);
        _dbContext.Add(guest2);
        _dbContext.Add(guest3);
        _dbContext.Add(bg1);
        _dbContext.Add(bg2);
        _dbContext.Add(bg3);
        await _dbContext.SaveChangesAsync();

        var cmd = new DeleteBookingGuestCmd(booking.Id, guest2.Id);
        await _mediator.Send(cmd);

        var proofPrimary = await _dbContext.BookingGuests.FirstAsync(q => q.BookingId == booking.Id && q.GuestId == guest3.Id);
        var proofNotPrimary = await _dbContext.BookingGuests.FirstAsync(q => q.BookingId == booking.Id && q.GuestId == guest1.Id);

        proofPrimary.IsPrincipal.ShouldBeTrue();
        proofNotPrimary.IsPrincipal.ShouldBeFalse();
    }
}