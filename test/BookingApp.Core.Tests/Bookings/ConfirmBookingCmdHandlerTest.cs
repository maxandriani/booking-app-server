using BookingApp.Core.Bookings.Commands;
using BookingApp.Core.Bookings.Exceptions;
using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.Data;
using BookingApp.Core.Places.Models;
using BookingApp.Core.Tests.Commons;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace BookingApp.Core.Tests.Bookings;

public class ConfirmBookingCmdHandlerTest : TestBase
{
    [Fact]
    public async Task Should_Throw_ValidateException_When_BookingId_is_Empty()
    {
        await Should.ThrowAsync<ValidationException>(
            () => _mediator.Send(new ConfirmBookingCmd(Guid.Empty)));
    }

    [Fact]
    public async Task Should_Throw_ResourceNotFoundException_when_booking_does_not_exists()
    {
        await Should.ThrowAsync<ResourceNotFoundException>(
            () => _mediator.Send(new ConfirmBookingCmd(Guid.NewGuid())));
    }

    public static IEnumerable<object[]> Should_Throw_BookingOverlapException_when_booking_has_overlap_schedules_data = new List<object[]>()
    {
        new object[] { DateTime.UtcNow, DateTime.UtcNow.AddDays(7) },
        new object[] { DateTime.UtcNow.AddDays(-7), DateTime.UtcNow },
        new object[] { DateTime.UtcNow.AddDays(7), DateTime.UtcNow.AddDays(14) },
        new object[] { DateTime.UtcNow.AddDays(-7), DateTime.UtcNow.AddDays(14) },
        new object[] { DateTime.UtcNow.AddDays(2), DateTime.UtcNow.AddDays(4) }
    };

    [Theory]
    [MemberData(nameof(Should_Throw_BookingOverlapException_when_booking_has_overlap_schedules_data))]
    public async Task Should_Throw_BookingOverlapException_when_booking_has_overlap_schedules(
        DateTime checkIn, DateTime checkOut)
    {
        var place = new Place() { Name = "Overlap Place", Address = "Overlap Place Street" };
        var booking = new Booking(Guid.NewGuid(), place.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(7), "", BookingStatusEnum.Confirmed);
        var overlap = new Booking(Guid.NewGuid(), place.Id, checkIn, checkOut, "");
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        dbContext.Add(place);
        dbContext.Add(booking);
        dbContext.Add(overlap);
        await dbContext.SaveChangesAsync();

        var cmd = new ConfirmBookingCmd(overlap.Id);
        await Should.ThrowAsync<BookingOverlapException>(
            () => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_confirm_a_booking()
    {
        var place = new Place() { Name = "Overlap Place", Address = "Overlap Place Street" };
        var booking = new Booking(Guid.NewGuid(), place.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(2), "", BookingStatusEnum.Confirmed);
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        dbContext.Add(place);
        dbContext.Add(booking);
        await dbContext.SaveChangesAsync();

        var cmd = new ConfirmBookingCmd(booking.Id);
        await _mediator.Send(cmd);

        var proof = await dbContext.Bookings.FirstAsync(q => q.Id == booking.Id);
        proof.Id.ShouldBe(booking.Id);
        proof.Status.ShouldBe(BookingStatusEnum.Confirmed);
    }
}