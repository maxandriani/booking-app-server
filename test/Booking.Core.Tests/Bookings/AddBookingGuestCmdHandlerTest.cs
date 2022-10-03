using Booking.Core.Bookings.Commands;
using Booking.Core.Commons.Exceptions;
using Booking.Core.Data;
using Booking.Core.Guests.Models;
using Booking.Core.Places.Models;
using Booking.Core.Tests.Commons;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Booking.Core.Tests.Bookings;

public class AddBookingGuestCmdHandlerTest : TestBase
{
    [Fact]
    public async Task Should_Throw_ValidationException_When_BookingId_Is_Empty()
    {
        var guest = new Guest() { Name = "Guest Name" };
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        dbContext.Add(guest);
        await dbContext.SaveChangesAsync();

        var cmd = new AddBookingGuestCmd(Guid.Empty, guest.Id);
        await Should.ThrowAsync<ValidationException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_Throw_ValidationException_When_GuestId_Is_Empty()
    {
        var place = new Place() { Name = "Jona's Place", Address = "Jona's Place Address" };
        var booking = new Booking.Core.Bookings.Models.Booking() { PlaceId = place.Id, CheckIn = DateTime.UtcNow, CheckOut = DateTime.UtcNow.AddDays(5), Status = Core.Bookings.Models.BookingStatusEnum.Confirmed };
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        dbContext.Add(place);
        dbContext.Add(booking);
        await dbContext.SaveChangesAsync();

        var cmd = new AddBookingGuestCmd(booking.Id, Guid.Empty);
        await Should.ThrowAsync<ValidationException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_Throw_ResourceNotFoundException_When_Booking_Is_Not_Found()
    {
        var guest = new Guest() { Name = "Guest Name" };
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        dbContext.Add(guest);
        await dbContext.SaveChangesAsync();

        var cmd = new AddBookingGuestCmd(Guid.NewGuid(), guest.Id);
        await Should.ThrowAsync<ResourceNotFoundException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_Throw_ResourceNotFoundException_When_Guest_Is_Not_Found()
    {
        var place = new Place() { Name = "Jona's Place", Address = "Jona's Place Address" };
        var booking = new Booking.Core.Bookings.Models.Booking() { PlaceId = place.Id, CheckIn = DateTime.UtcNow, CheckOut = DateTime.UtcNow.AddDays(5), Status = Core.Bookings.Models.BookingStatusEnum.Confirmed };
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        dbContext.Add(place);
        dbContext.Add(booking);
        await dbContext.SaveChangesAsync();

        var cmd = new AddBookingGuestCmd(booking.Id, Guid.NewGuid());
        await Should.ThrowAsync<ResourceNotFoundException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_Add_a_Guest_to_a_Booking()
    {
        var place = new Place() { Name = "Jona's Place", Address = "Jona's Place Address" };
        var booking = new Booking.Core.Bookings.Models.Booking() { PlaceId = place.Id, CheckIn = DateTime.UtcNow, CheckOut = DateTime.UtcNow.AddDays(5), Status = Core.Bookings.Models.BookingStatusEnum.Confirmed };
        var guest = new Guest() { Name = "Guest Name" };
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        dbContext.Add(place);
        dbContext.Add(booking);
        dbContext.Add(guest);
        await dbContext.SaveChangesAsync();

        var cmd = new AddBookingGuestCmd(booking.Id, guest.Id);
        await _mediator.Send(cmd);

        dbContext.BookingGuests
            .Any(q => q.BookingId == booking.Id && q.GuestId == guest.Id)
            .ShouldBe(true);
    }

    [Fact]
    public async Task Should_Add_first_Guest_as_Primary_Guest()
    {
        var place = new Place() { Name = "Jona's Place", Address = "Jona's Place Address" };
        var booking = new Booking.Core.Bookings.Models.Booking() { PlaceId = place.Id, CheckIn = DateTime.UtcNow, CheckOut = DateTime.UtcNow.AddDays(5), Status = Core.Bookings.Models.BookingStatusEnum.Confirmed };
        var guest = new Guest() { Name = "Guest Name" };
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        dbContext.Add(place);
        dbContext.Add(booking);
        dbContext.Add(guest);
        await dbContext.SaveChangesAsync();

        var cmd = new AddBookingGuestCmd(booking.Id, guest.Id);
        await _mediator.Send(cmd);

        dbContext.BookingGuests
            .Any(q => q.BookingId == booking.Id && q.GuestId == guest.Id && q.IsPrincipal == true)
            .ShouldBe(true);
    }

    [Fact]
    public async Task Should_Add_second_Guest_as_Not_Primary_Guest()
    {
        var place = new Place() { Name = "Jona's Place", Address = "Jona's Place Address" };
        var booking = new Booking.Core.Bookings.Models.Booking() { PlaceId = place.Id, CheckIn = DateTime.UtcNow, CheckOut = DateTime.UtcNow.AddDays(5), Status = Core.Bookings.Models.BookingStatusEnum.Confirmed };
        var guest1 = new Guest() { Name = "Guest Name 1" };
        var guest2 = new Guest() { Name = "Guest Name 2" };
        var guest3 = new Guest() { Name = "Guest Name 3" };
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        dbContext.Add(place);
        dbContext.Add(booking);
        dbContext.Add(guest1);
        dbContext.Add(guest2);
        dbContext.Add(guest3);
        await dbContext.SaveChangesAsync();

        await _mediator.Send(new AddBookingGuestCmd(booking.Id, guest1.Id));
        await _mediator.Send(new AddBookingGuestCmd(booking.Id, guest2.Id));
        await _mediator.Send(new AddBookingGuestCmd(booking.Id, guest3.Id));

        var bg1 = await dbContext.BookingGuests.FirstAsync(q => q.BookingId == booking.Id && q.GuestId == guest1.Id);
        var bg2 = await dbContext.BookingGuests.FirstAsync(q => q.BookingId == booking.Id && q.GuestId == guest2.Id);
        var bg3 = await dbContext.BookingGuests.FirstAsync(q => q.BookingId == booking.Id && q.GuestId == guest3.Id);

        bg1.IsPrincipal.ShouldBe(true);
        bg2.IsPrincipal.ShouldBe(false);
        bg3.IsPrincipal.ShouldBe(false);
    }
}