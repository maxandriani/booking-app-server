using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Bookings.Queries;
using BookingApp.Core.GuestContacts.Models;
using BookingApp.Core.Guests.Models;
using BookingApp.Core.Places.Models;
using BookingApp.Core.Tests.Commons;
using Shouldly;

namespace BookingApp.Core.Tests.Bookings;

public class SearchBookingsQueryHandlerTest : TestBase
{
    private static Place Place1 = new Place(Guid.NewGuid(), "Search Booking Place", "23th, Search Street");
    private static Place Place2 = new Place(Guid.NewGuid(), "Search Booking 2", "45th, Search Street");
    private static Guest Guest1 = new Guest(
        Guid.NewGuid(),
        "Guest 1",
        new List<GuestContact>()
        {
            new(Guid.NewGuid(), Guid.Empty, GuestContactTypeEnum.Email, "guest.1@gmail.com"),
            new(Guid.NewGuid(), Guid.Empty, GuestContactTypeEnum.Phone, "+55 47 99669-9933")
        }
        .AsReadOnly()
    );

    private static Guest Guest2 = new Guest(
        Guid.NewGuid(),
        "Guest 2",
        new List<GuestContact>()
        {
            new(Guid.NewGuid(), Guid.Empty, GuestContactTypeEnum.Email, "guest.2@gmail.com")
        }.AsReadOnly()
    );

    private static Guest Guest3 = new Guest(
        Guid.NewGuid(),
        "Guest 3",
        new List<GuestContact>()
        {
            new(Guid.NewGuid(), Guid.Empty, GuestContactTypeEnum.Email, "guest.3@gmail.com"),
            new(Guid.NewGuid(), Guid.Empty, GuestContactTypeEnum.Phone, "+55 47 66885-8822")
        }.AsReadOnly()
    );

    private static List<Booking> Bookings = new()
    {
        new (Guid.NewGuid(), Place1.Id, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(2), "Booking 1", BookingStatusEnum.Unknown),
        new (Guid.NewGuid(), Place1.Id, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(2), "Booking 2", BookingStatusEnum.Pending),
        new (Guid.NewGuid(), Place1.Id, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(2), "Booking 3", BookingStatusEnum.Confirmed),
        new (Guid.NewGuid(), Place1.Id, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(2), "Booking 4", BookingStatusEnum.Cancelled),
        new (Guid.NewGuid(), Place1.Id, DateTime.UtcNow.Date.AddDays(3), DateTime.UtcNow.Date.AddDays(6), "Booking 5", BookingStatusEnum.Confirmed),
        new (Guid.NewGuid(), Place2.Id, DateTime.UtcNow.Date.AddDays(7), DateTime.UtcNow.Date.AddDays(10), "Booking 6", BookingStatusEnum.Confirmed),
        new (Guid.NewGuid(), Place2.Id, DateTime.UtcNow.Date.AddDays(11), DateTime.UtcNow.Date.AddDays(12), "Booking 7", BookingStatusEnum.Confirmed),
        new (Guid.NewGuid(), Place2.Id, DateTime.UtcNow.Date.AddDays(13), DateTime.UtcNow.Date.AddDays(14), "Booking 8", BookingStatusEnum.Confirmed),
        new (Guid.NewGuid(), Place2.Id, DateTime.UtcNow.Date.AddDays(15), DateTime.UtcNow.Date.AddDays(16), "Booking 9", BookingStatusEnum.Confirmed)
    };

    private static List<BookingGuest> Guests = Bookings.SelectMany((booking, x) => (x % 3) switch
    {
        0 => new List<BookingGuest>() { new(booking.Id, Guest1.Id, true) },
        1 => new List<BookingGuest>() { new(booking.Id, Guest2.Id, true), new(booking.Id, Guest3.Id, false) },
        2 => new List<BookingGuest>() { new(booking.Id, Guest3.Id, true) },
        _ => throw new ArgumentOutOfRangeException("Impossível vir 3 como resultado de módulo 3")
    }).ToList();

    public SearchBookingsQueryHandlerTest() : base()
    {
        _dbContext.AddRange(
            Place1, Place2,
            Guest1, Guest2, Guest3
        );
        _dbContext.AddRange(Bookings);
        _dbContext.AddRange(Guests);
        _dbContext.SaveChanges();
    }

    public static List<object[]> Should_filter_bookings_by_place_data = new()
    {
        new object[] { Place1, new List<Booking>() { Bookings[0], Bookings[1], Bookings[2], Bookings[3], Bookings[4] } },
        new object[] { Place2, new List<Booking>() { Bookings[5], Bookings[6], Bookings[7], Bookings[8] } }
    };

    [Theory]
    [MemberData(nameof(Should_filter_bookings_by_place_data))]
    public async Task Should_filter_bookings_by_place(Place place, List<Booking> results)
    {
        var cmd = new SearchBookingsQuery() { ByPlace = place.Id };
        var proof = await _mediator.Send(cmd);

        proof.TotalCount.ShouldBe(results.Count());
        await foreach(var booking in proof.Items)
        {
            results.ShouldContain(q => q.Id == booking.Id);
            booking.PlaceId.ShouldBe(place.Id);
            booking.Place.Name.ShouldBe(place.Name);
        }
    }

    public static List<object[]> Should_filter_bookings_by_matching_date_data = new()
    {
        new object[] { DateTime.UtcNow.Date, new List<Booking>() { Bookings[0], Bookings[1], Bookings[2], Bookings[3] } },
        new object[] { DateTime.UtcNow.Date.AddDays(2), new List<Booking>() { Bookings[0], Bookings[1], Bookings[2], Bookings[3] } },
        new object[] { DateTime.UtcNow.Date.AddDays(4), new List<Booking>() { Bookings[4] } },
        new object[] { DateTime.UtcNow.Date.AddDays(8), new List<Booking>() { Bookings[5] } },
        new object[] { DateTime.UtcNow.Date.AddDays(12), new List<Booking>() { Bookings[6] } }
    };

    [Theory]
    [MemberData(nameof(Should_filter_bookings_by_matching_date_data))]
    public async Task Should_filter_bookings_by_matching_date(DateTime date, List<Booking> results)
    {
        var cmd = new SearchBookingsQuery() { Date = date.Date };
        var proof = await _mediator.Send(cmd);

        proof.TotalCount.ShouldBe(results.Count());
        await foreach(var booking in proof.Items)
        {
            results.ShouldContain(q => q.Id == booking.Id);
            booking.CheckIn.ShouldBeLessThanOrEqualTo(date);
            booking.CheckOut.ShouldBeGreaterThanOrEqualTo(date);
        }
    }

    public static List<object[]> Should_filter_bookings_by_status_data = new()
    {
        new object[] { BookingStatusEnum.Unknown, new List<Booking>() { Bookings[0] } },
        new object[] { BookingStatusEnum.Pending, new List<Booking>() { Bookings[1] } },
        new object[] { BookingStatusEnum.Confirmed, new List<Booking>() { Bookings[2], Bookings[4], Bookings[5], Bookings[6], Bookings[7], Bookings[8] } },
        new object[] { BookingStatusEnum.Cancelled, new List<Booking>() { Bookings[3] } }
    };

    [Theory]
    [MemberData(nameof(Should_filter_bookings_by_status_data))]
    public async Task Should_filter_bookings_by_status(BookingStatusEnum status, List<Booking> results)
    {
        var cmd = new SearchBookingsQuery() { Status = status };
        var proof = await _mediator.Send(cmd);

        proof.TotalCount.ShouldBe(results.Count());
        await foreach(var booking in proof.Items)
        {
            results.ShouldContain(q => q.Id == booking.Id);
            booking.Status.ShouldBe(status);
        }
    }

    public static List<object[]> Should_filter_bookings_by_description_data = new()
    {
        new object[] { "Booking 1", new List<Booking>() { Bookings[0] } },
        new object[] { "Booking 5", new List<Booking>() { Bookings[4] } },
        new object[] { "Booking 9", new List<Booking>() { Bookings[8] } }
    };

    [Theory]
    [MemberData(nameof(Should_filter_bookings_by_description_data))]
    public async Task Should_filter_bookings_by_description(string description, List<Booking> results)
    {
        var cmd = new SearchBookingsQuery() { Search = description };
        var proof = await _mediator.Send(cmd);

        proof.TotalCount.ShouldBe(results.Count());
        await foreach(var booking in proof.Items)
        {
            results.ShouldContain(q => q.Id == booking.Id);
        }
    }

    public static List<object[]> Should_filter_bookings_by_guest_name_data = new()
    {
        new object[] { "Guest 1", new List<(Booking, string)>() { (Bookings[0], "Guest 1"), (Bookings[3], "Guest 1"), (Bookings[6], "Guest 1") } },
        new object[] { "Guest 2", new List<(Booking, string)>() { (Bookings[1], "Guest 2"), (Bookings[4], "Guest 2"), (Bookings[7], "Guest 2") } },
        new object[] { "Guest 3", new List<(Booking, string)>() { (Bookings[1], "Guest 2"), (Bookings[2], "Guest 3"), (Bookings[4], "Guest 2"), (Bookings[5], "Guest 3"), (Bookings[7], "Guest 2"), (Bookings[8], "Guest 3") } }
    };

    [Theory]
    [MemberData(nameof(Should_filter_bookings_by_guest_name_data))]
    public async Task Should_filter_bookings_by_guest_name(string search, List<(Booking Booking, string Name)> results)
    {
        var cmd = new SearchBookingsQuery() { Search = search };
        var proof = await _mediator.Send(cmd);

        proof.TotalCount.ShouldBe(results.Count());
        await foreach(var booking in proof.Items)
        {
            var result = results.FirstOrDefault(q => q.Booking.Id == booking.Id);
            result.Booking.Id.ShouldBe(booking.Id);
            booking.Guest?.Name.ShouldBe(result.Name);
        }
    }
}