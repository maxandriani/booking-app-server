using BookingApp.Core.Tests.Commons;
using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Bookings.Commands;
using Shouldly;
using FluentValidation;
using BookingApp.Core.Places.Models;
using BookingApp.Core.Commons.Exceptions;
using Microsoft.EntityFrameworkCore;
using BookingApp.Core.Bookings.Exceptions;

namespace BookingApp.Core.Tests.Bookings;

public class UpdateBookingCmdHandlerTest : TestBase
{
    [Fact]
    public async Task Should_throw_ValidateException_when_booking_id_is_empty()
    {
        var cmd = new UpdateBookingCmd(Guid.Empty, Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddDays(1), "");
        await Should.ThrowAsync<ValidationException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_throw_ValidateException_when_place_id_is_empty()
    {
        var cmd = new UpdateBookingCmd(Guid.NewGuid(), Guid.Empty, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), "");
        await Should.ThrowAsync<ValidationException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_throw_ValidateException_when_checkout_is_greater_than_checkin()
    {
        var cmd = new UpdateBookingCmd(Guid.NewGuid(), Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddDays(-2), "");
        await Should.ThrowAsync<ValidationException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_throw_ResourceNotFoundException_when_booking_does_not_exists()
    {
        var place = new Place(Guid.NewGuid(), "UpdateBooking Place", "23th, UpdateBooking Street");
        _dbContext.Add(place);
        await _dbContext.SaveChangesAsync();

        var cmd = new UpdateBookingCmd(Guid.NewGuid(), place.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), "");
        await Should.ThrowAsync<ResourceNotFoundException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_throw_ResourceNotFoundException_when_place_does_not_exists()
    {
        var place = new Place(Guid.NewGuid(), "UpdateBooking Place", "23th, UpdateBooking Street");
        var booking = new Booking(Guid.NewGuid(), place.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), "");
        _dbContext.Add(place);
        _dbContext.Add(booking);
        await _dbContext.SaveChangesAsync();

        var cmd = new UpdateBookingCmd(booking.Id, Guid.NewGuid(), DateTime.UtcNow, DateTime.UtcNow.AddDays(2), "");
        await Should.ThrowAsync<ResourceNotFoundException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_update_booking_description()
    {
        var description = "Nova descrição";
        var place = new Place(Guid.NewGuid(), "UpdateBooking Place", "23th, UpdateBooking Street");
        var booking = new Booking(Guid.NewGuid(), place.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), "");
        _dbContext.Add(place);
        _dbContext.Add(booking);
        await _dbContext.SaveChangesAsync();

        var cmd = new UpdateBookingCmd(booking.Id, place.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), description);
        await _mediator.Send(cmd);

        var proof = await _dbContext.Bookings.FirstAsync(q => q.Id == booking.Id);
        proof.Description.ShouldBe(description);
    }

    [Fact]
    public async Task Should_update_booking_place()
    {
        var description = "Nova descrição";
        var place = new Place(Guid.NewGuid(), "UpdateBooking Place", "23th, UpdateBooking Street");
        var place2 = new Place(Guid.NewGuid(), "New Place", "34th, New Place Street");
        var booking = new Booking(Guid.NewGuid(), place.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), "");
        _dbContext.Add(place);
        _dbContext.Add(place2);
        _dbContext.Add(booking);
        await _dbContext.SaveChangesAsync();

        var cmd = new UpdateBookingCmd(booking.Id, place2.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), description);
        await _mediator.Send(cmd);

        var proof = await _dbContext.Bookings.FirstAsync(q => q.Id == booking.Id);
        proof.PlaceId.ShouldBe(place2.Id);
    }

    [Fact]
    public async Task Should_update_booking_dates()
    {
        var place = new Place(Guid.NewGuid(), "UpdateBooking Place", "23th, UpdateBooking Street");
        var booking = new Booking(Guid.NewGuid(), place.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), "");
        _dbContext.Add(place);
        _dbContext.Add(booking);
        await _dbContext.SaveChangesAsync();

        var cmd = new UpdateBookingCmd(booking.Id, place.Id, DateTime.UtcNow.AddDays(6), DateTime.UtcNow.AddDays(12), "");
        await _mediator.Send(cmd);

        var proof = await _dbContext.Bookings.FirstAsync(q => q.Id == booking.Id);
        proof.CheckIn.ShouldBe(cmd.CheckIn);
        proof.CheckOut.ShouldBe(cmd.CheckOut);
    }

    public static IEnumerable<object[]> Booking_Read_Only_States = new List<object[]>()
    {
        new object[] { BookingStatusEnum.Cancelled },
        new object[] { BookingStatusEnum.Confirmed }
    };

    [Theory]
    [MemberData(nameof(Booking_Read_Only_States))]
    public async Task Should_throw_BookingReadOnlyStateException_when_try_update_dates_on_read_only_state(BookingStatusEnum status)
    {
        var place = new Place(Guid.NewGuid(), "UpdateBooking Place", "23th, UpdateBooking Street");
        var booking = new Booking(Guid.NewGuid(), place.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), "", status);
        _dbContext.Add(place);
        _dbContext.Add(booking);
        await _dbContext.SaveChangesAsync();

        var cmd = new UpdateBookingCmd(booking.Id, place.Id, DateTime.UtcNow.AddDays(6), DateTime.UtcNow.AddDays(12), "");
        await Should.ThrowAsync<BookingReadOnlyStateException>(() => _mediator.Send(cmd));
    }

    [Theory]
    [MemberData(nameof(Booking_Read_Only_States))]
    public async Task Should_throw_BookingReadOnlyStateException_when_try_update_place_on_read_only_state(BookingStatusEnum status)
    {
        var place = new Place(Guid.NewGuid(), "UpdateBooking Place", "23th, UpdateBooking Street");
        var place2 = new Place(Guid.NewGuid(), "New Place", "34th, New Place Street");
        var booking = new Booking(Guid.NewGuid(), place.Id, DateTime.UtcNow.Date, DateTime.UtcNow.AddDays(2).Date, "", status);
        _dbContext.Add(place);
        _dbContext.Add(place2);
        _dbContext.Add(booking);
        await _dbContext.SaveChangesAsync();

        var cmd = new UpdateBookingCmd(booking.Id, place2.Id, DateTime.UtcNow.Date, DateTime.UtcNow.AddDays(2).Date, "");
        await Should.ThrowAsync<BookingReadOnlyStateException>(() => _mediator.Send(cmd));
    }
}