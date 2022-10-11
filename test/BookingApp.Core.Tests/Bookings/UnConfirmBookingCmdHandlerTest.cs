using BookingApp.Core.Bookings.Commands;
using BookingApp.Core.Bookings.Exceptions;
using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.Places.Models;
using BookingApp.Core.Tests.Commons;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace BookingApp.Core.Tests.Bookings;

public class UnConfirmBookingCmdHandlerTest : TestBase
{
    [Fact]
    public async Task Should_throw_ValidationException_when_booking_id_is_empty()
    {
        var cmd = new UnConfirmBookingCmd(Guid.Empty);
        await Should.ThrowAsync<ValidationException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_throw_ResourceNotFoundException_when_booking_does_not_exists()
    {
        var cmd = new UnConfirmBookingCmd(Guid.NewGuid());
        await Should.ThrowAsync<ResourceNotFoundException>(() => _mediator.Send(cmd));
    }

    public static IEnumerable<object[]> Should_throw_BookingNotConfirmedException_when_booking_is_not_confirmed_data = new List<object[]>()
    {
        new object[] { BookingStatusEnum.Unknown },
        new object[] { BookingStatusEnum.Pending },
        new object[] { BookingStatusEnum.Cancelled }
    };

    [Theory]
    [MemberData(nameof(Should_throw_BookingNotConfirmedException_when_booking_is_not_confirmed_data))]
    public async Task Should_throw_BookingNotConfirmedException_when_booking_is_not_confirmed(BookingStatusEnum status)
    {
        var place = new Place(Guid.NewGuid(), "UnConfirm Place", "23th, UnConfirm Street.");
        var booking = new Booking(Guid.NewGuid(), place.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(3), null, status);
        _dbContext.Add(place);
        _dbContext.Add(booking);
        await _dbContext.SaveChangesAsync();

        var cmd = new UnConfirmBookingCmd(booking.Id);
        await Should.ThrowAsync<BookingNotConfirmedException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_un_confirm_a_booking()
    {
        var place = new Place(Guid.NewGuid(), "UnConfirm Place", "23th, UnConfirm Street.");
        var booking = new Booking(Guid.NewGuid(), place.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(3), null, BookingStatusEnum.Confirmed);
        _dbContext.Add(place);
        _dbContext.Add(booking);
        await _dbContext.SaveChangesAsync();

        var cmd = new UnConfirmBookingCmd(booking.Id);
        await _mediator.Send(cmd);

        var proof = await _dbContext.Bookings.FirstAsync(q => q.Id == booking.Id);
        proof.Status.ShouldBe(BookingStatusEnum.Pending);
    }
}