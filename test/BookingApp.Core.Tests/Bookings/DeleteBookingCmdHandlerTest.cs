using BookingApp.Core.Bookings.Commands;
using BookingApp.Core.Bookings.Exceptions;
using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.Guests.Models;
using BookingApp.Core.Places.Models;
using BookingApp.Core.Tests.Commons;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace BookingApp.Core.Tests.Bookings;

public class DeleteBookingCmdHandlerTest : TestBase
{
    [Fact]
    public async Task Should_throw_ValidationException_when_booking_id_is_empty()
    {
        var cmd = new DeleteBookingCmd(Guid.Empty);
        await Should.ThrowAsync<ValidationException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_throw_ResourceNotFoundException_when_booking_does_not_exists()
    {
        var cmd = new DeleteBookingCmd(Guid.NewGuid());
        await Should.ThrowAsync<ResourceNotFoundException>(() => _mediator.Send(cmd));
    }

    public static IEnumerable<object[]> Should_throw_BookingReadOnlyStareException_when_booking_is_readonly_state_data = new List<object[]>()
    {
        new object[] { BookingStatusEnum.Confirmed },
        new object[] { BookingStatusEnum.Cancelled },
};

    [Theory]
    [MemberData(nameof(Should_throw_BookingReadOnlyStareException_when_booking_is_readonly_state_data))]
    public async Task Should_throw_BookingReadOnlyStareException_when_booking_is_readonly_state(BookingStatusEnum status)
    {
        var place = new Place(Guid.NewGuid(), "Jonas' Delete House", "23th, Jonas Street.");
        var booking = new Booking(Guid.NewGuid(), place.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(7), "Deletar-me-ei", status);
        _dbContext.Add(place);
        _dbContext.Add(booking);
        await _dbContext.SaveChangesAsync();

        var cmd = new DeleteBookingCmd(booking.Id);
        await Should.ThrowAsync<BookingReadOnlyStateException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_delete_a_booking_and_guestBooking_references()
    {
        var place = new Place(Guid.NewGuid(), "Jonas' Delete House", "23th, Jonas Street.");
        var booking = new Booking(Guid.NewGuid(), place.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(7), "Deletar-me-ei");
        var guest1 = new Guest(Guid.NewGuid(), "Jonas Guest 1");
        var guest2 = new Guest(Guid.NewGuid(), "Maria Guest 2");
        var bg1 = new BookingGuest(booking.Id, guest1.Id, true);
        var bg2 = new BookingGuest(booking.Id, guest2.Id, false);
        _dbContext.Add(place);
        _dbContext.Add(guest1);
        _dbContext.Add(guest2);
        _dbContext.Add(booking);
        _dbContext.Add(bg1);
        _dbContext.Add(bg2);
        await _dbContext.SaveChangesAsync();

        var cmd = new DeleteBookingCmd(booking.Id);
        await _mediator.Send(cmd);

        var bookingProof = await _dbContext.Bookings.AnyAsync(q => q.Id == booking.Id);
        var bg1Proof = await _dbContext.BookingGuests.AnyAsync(q => q.BookingId == booking.Id && q.GuestId == guest1.Id);
        var bg2Proof = await _dbContext.BookingGuests.AnyAsync(q => q.BookingId == booking.Id && q.GuestId == guest2.Id);

        bookingProof.ShouldBeFalse();
        bg1Proof.ShouldBeFalse();
        bg2Proof.ShouldBeFalse();
    }
}