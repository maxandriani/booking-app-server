using FluentValidation;
using BookingApp.Core.Bookings.Commands;
using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.Data;
using BookingApp.Core.Places.Models;
using BookingApp.Core.Tests.Commons;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace BookingApp.Core.Tests.Bookings;

public class CancelBookingCmdHandlerTest : TestBase
{

    [Fact]
    public async Task Should_Cancel_Confirmed_Booking_Record()
    {
        var place = new Place() { Name = "Jonas Place", Address = "Jonas Address" };
        var booking = new Booking() { PlaceId = place.Id, CheckIn = DateTime.UtcNow, CheckOut = DateTime.UtcNow.AddDays(4), Status = BookingStatusEnum.Confirmed };
        var dbContext = _injector.GetRequiredService<BookingDbContext>();

        dbContext.Add(place);
        dbContext.Add(booking);
        await dbContext.SaveChangesAsync();

        var cmd = new CancelBookingCmd(booking.Id);
        await _mediator.Send(cmd);

        var proof = await dbContext.Bookings.FirstAsync(q => q.Id == booking.Id);
        proof.Status.ShouldBe(BookingStatusEnum.Cancelled);
    }

    
    [Fact]
    public async Task Should_Thrown_BusinessException_If_Cancel_Not_Confirmed_Booking_Record()
    {
        var place = new Place() { Name = "Jonas Place", Address = "Jonas Address" };
        var booking = new Booking() { PlaceId = place.Id, CheckIn = DateTime.UtcNow, CheckOut = DateTime.UtcNow.AddDays(4), Status = BookingStatusEnum.Cancelled };
        var dbContext = _injector.GetRequiredService<BookingDbContext>();

        dbContext.Add(place);
        dbContext.Add(booking);
        await dbContext.SaveChangesAsync();

        var cmd = new CancelBookingCmd(booking.Id);
        await Should.ThrowAsync<BusinessException>(() => _mediator.Send(cmd));
    }

    
    [Fact]
    public async Task Should_Thrown_ValidationException_If_Booking_Id_Is_Empty()
    {
        var cmd = new CancelBookingCmd(Guid.Empty);
        await Should.ThrowAsync<ValidationException>(() => _mediator.Send(cmd));
    }

    
    [Fact]
    public async Task Should_Thrown_ResourceNotFoundException_If_Booking_Does_Not_exists()
    {
        var cmd = new CancelBookingCmd(Guid.NewGuid());
        await Should.ThrowAsync<ResourceNotFoundException>(() => _mediator.Send(cmd));
    }

}