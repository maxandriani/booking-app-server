using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Bookings.Queries;
using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.GuestContacts.Models;
using BookingApp.Core.Guests.Models;
using BookingApp.Core.Places.Models;
using BookingApp.Core.Tests.Commons;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Shouldly;

namespace BookingApp.Core.Bookings;

public class GetBookingByKeyQueryHandlerTest : TestBase
{
    private static Guest GuestA = new(Guid.NewGuid(), "Guest A");
    private static Guest GuestB = new(Guid.NewGuid(), "Guest B");
    private static GuestContact GuestAContact1 = new(Guid.NewGuid(), GuestA.Id, GuestContactTypeEnum.Email, "guesta@gmail.com");
    private static GuestContact GuestAContact2 = new(Guid.NewGuid(), GuestA.Id, GuestContactTypeEnum.Phone, "+55 47 99944-3333");
    private static GuestContact GuestBContact1 = new(Guid.NewGuid(), GuestB.Id, GuestContactTypeEnum.Email, "guestb@gmail.com");
    private static Place Place = new(Guid.NewGuid(), "GetByKey Place", "34th, GetByKey Street");
    private static Booking Booking = new(Guid.NewGuid(), Place.Id, DateTime.UtcNow, DateTime.UtcNow.AddDays(4), "Description", BookingStatusEnum.Confirmed);
    private static BookingGuest BookingGuestA = new BookingGuest(Booking.Id, GuestA.Id, true);
    private static BookingGuest BookingGuestB = new BookingGuest(Booking.Id, GuestB.Id, false);

    public GetBookingByKeyQueryHandlerTest() : base()
    {
        _dbContext.AddRange(GuestA, GuestB, GuestAContact1, GuestAContact2, GuestBContact1, Place, Booking, BookingGuestA, BookingGuestB);
        _dbContext.SaveChanges();
    }

    [Fact]
    public async Task Should_throw_ValidationException_when_booking_id_is_null()
    {
        var cmd = new GetBookingByKeyQuery(Guid.Empty);
        await Should.ThrowAsync<ValidationException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_throw_ResourceNotFoundException_when_booking_does_not_exists()
    {
        var cmd = new GetBookingByKeyQuery(Guid.NewGuid());
        await Should.ThrowAsync<ResourceNotFoundException>(() => _mediator.Send(cmd));
    }

    [Fact]
    public async Task Should_return_all_booking_properties()
    {
        var cmd = new GetBookingByKeyQuery(Booking.Id);
        var proof = await _mediator.Send(cmd);

        proof.Id.ShouldBe(Booking.Id);
        proof.PlaceId.ShouldBe(Booking.PlaceId);
        proof.CheckIn.ShouldBe(Booking.CheckIn);
        proof.CheckOut.ShouldBe(Booking.CheckOut);
        proof.Description.ShouldBe(Booking.Description);
        proof.Status.ShouldBe(Booking.Status);
    }

    [Fact]
    public async Task Should_return_all_place_properties()
    {
        var cmd = new GetBookingByKeyQuery(Booking.Id);
        var proof = await _mediator.Send(cmd);

        proof.Place.ShouldNotBeNull();
        proof.Place.Id.ShouldBe(Place.Id);
        proof.Place.Name.ShouldBe(Place.Name);
        proof.Place.Address.ShouldBe(Place.Address);
    }

    [Fact]
    public async Task Should_return_all_guest_properties()
    {
        var cmd = new GetBookingByKeyQuery(Booking.Id);
        var proof = await _mediator.Send(cmd);
        
        var guestProof = proof.Guests.First(q => q.IsPrincipal);
        guestProof.Name.ShouldBe(GuestA.Name);
    }

    [Fact]
    public async Task Should_return_all_guest_contact_properties()
    {
        var cmd = new GetBookingByKeyQuery(Booking.Id);
        var proof = await _mediator.Send(cmd);

        var guestProof = proof.Guests.First(q => q.IsPrincipal);
        guestProof.Contacts.ShouldContain(q => q.Type == GuestAContact1.Type && q.Value == GuestAContact1.Value);
    }
}