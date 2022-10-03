using System.ComponentModel.DataAnnotations;
using Booking.Core.Bookings.Models;
using Booking.Core.Data;
using Booking.Core.Places.Models;
using Booking.Core.Places.Queries;
using Booking.Core.Tests.Commons;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Booking.Core.Tests.Places;

public class SearchAvailablePlacesForBookingQueryHandlerTests : TestBase
{
    private static List<Place> PLACES = new() {
        new Place(new Guid("7fd96d44-b61f-4d1f-bba0-d60ac8488875"), "Jonas House 1", "Jona's Street 1"),
        new Place(new Guid("fb2709c2-13a4-4559-83cd-a340f3e53e0b"), "Jonas House 2", "Jona's Street 2"),
        new Place(new Guid("2f9aae96-1026-4d76-b8ee-431eb2d32687"), "Jonas House 3", "Jona's Street 3"),
        new Place(new Guid("de2b05b4-de6c-4e4c-9ffb-6361be0e7425"), "Jonas House 4", "Jona's Street 4")
    };
    private static List<Booking.Core.Bookings.Models.Booking> CONFIRMED_BOOKINGS = new() {
        new Booking.Core.Bookings.Models.Booking(new Guid("81765f35-d720-444c-8007-582e322099af"), new Guid("7fd96d44-b61f-4d1f-bba0-d60ac8488875"), new DateTime(2022, 10, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 10, 0, 0, 0, DateTimeKind.Utc), null, BookingStatusEnum.Confirmed),
        new Booking.Core.Bookings.Models.Booking(new Guid("e89d0108-7f9d-45b5-9646-dcca4a62e723"), new Guid("7fd96d44-b61f-4d1f-bba0-d60ac8488875"), new DateTime(2022, 10, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 19, 0, 0, 0, DateTimeKind.Utc), null, BookingStatusEnum.Confirmed),
        new Booking.Core.Bookings.Models.Booking(new Guid("66062aab-1270-45cb-b5f1-6288006298db"), new Guid("7fd96d44-b61f-4d1f-bba0-d60ac8488875"), new DateTime(2022, 10, 20, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 30, 0, 0, 0, DateTimeKind.Utc), null, BookingStatusEnum.Confirmed),
        new Booking.Core.Bookings.Models.Booking(new Guid("4abbd580-00d2-42ae-90ee-62e9d4d2910e"), new Guid("fb2709c2-13a4-4559-83cd-a340f3e53e0b"), new DateTime(2022, 10, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 20, 0, 0, 0, DateTimeKind.Utc), null, BookingStatusEnum.Confirmed),
        new Booking.Core.Bookings.Models.Booking(new Guid("8c34bfb5-07db-4b2f-a871-4c0803b356ae"), new Guid("2f9aae96-1026-4d76-b8ee-431eb2d32687"), new DateTime(2022, 10, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 25, 0, 0, 0, DateTimeKind.Utc), null, BookingStatusEnum.Confirmed),
        new Booking.Core.Bookings.Models.Booking(new Guid("22038429-6860-456d-aabf-dc8467ad7da1"), new Guid("de2b05b4-de6c-4e4c-9ffb-6361be0e7425"), new DateTime(2022, 10, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 11, 0, 0, 0, DateTimeKind.Utc), null, BookingStatusEnum.Confirmed)
    };
    private static List<Booking.Core.Bookings.Models.Booking> UNCONFIRMED_BOOKINGS = new() {
        new Booking.Core.Bookings.Models.Booking(new Guid("727cb81c-af96-44b3-884e-6f5af6058125"), new Guid("7fd96d44-b61f-4d1f-bba0-d60ac8488875"), new DateTime(2023, 10, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 11, 10, 0, 0, 0, DateTimeKind.Utc), null, BookingStatusEnum.Pending),
        new Booking.Core.Bookings.Models.Booking(new Guid("9440b551-8fbc-4d28-af1d-e8e190fd35a2"), new Guid("7fd96d44-b61f-4d1f-bba0-d60ac8488875"), new DateTime(2023, 10, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 11, 19, 0, 0, 0, DateTimeKind.Utc), null, BookingStatusEnum.Pending),
        new Booking.Core.Bookings.Models.Booking(new Guid("5d606613-1cb6-4266-9b75-7b781fad992c"), new Guid("7fd96d44-b61f-4d1f-bba0-d60ac8488875"), new DateTime(2023, 10, 20, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 11, 30, 0, 0, 0, DateTimeKind.Utc), null, BookingStatusEnum.Pending),
        new Booking.Core.Bookings.Models.Booking(new Guid("852049e3-2539-4465-9de5-2a251ffaffef"), new Guid("fb2709c2-13a4-4559-83cd-a340f3e53e0b"), new DateTime(2023, 10, 5, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 11, 20, 0, 0, 0, DateTimeKind.Utc), null, BookingStatusEnum.Pending),
        new Booking.Core.Bookings.Models.Booking(new Guid("26999c34-9bd0-453c-bffb-9367c2c19c52"), new Guid("2f9aae96-1026-4d76-b8ee-431eb2d32687"), new DateTime(2023, 10, 11, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 11, 25, 0, 0, 0, DateTimeKind.Utc), null, BookingStatusEnum.Pending),
        new Booking.Core.Bookings.Models.Booking(new Guid("a8897684-4bd0-4e0c-8ca0-4d3d78fdc3f3"), new Guid("de2b05b4-de6c-4e4c-9ffb-6361be0e7425"), new DateTime(2023, 10, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 11, 11, 0, 0, 0, DateTimeKind.Utc), null, BookingStatusEnum.Pending)
    };
    private static List<Booking.Core.Bookings.Models.Booking> ALL_BOOKINGS = CONFIRMED_BOOKINGS.Union(UNCONFIRMED_BOOKINGS).ToList();

    public SearchAvailablePlacesForBookingQueryHandlerTests() : base()
    {
        var db = _injector.GetRequiredService<BookingDbContext>();
        db.AddRange(PLACES);
        db.AddRange(ALL_BOOKINGS);
        db.SaveChanges();
    }

    public static List<object[]> Should_return_all_places_given_no_booking_conflict_date_range_data = new() {
        new object[] { new DateTime(2022, 9, 10, 0, 0, 0, DateTimeKind.Utc),  new DateTime(2022, 9, 30, 0, 0, 0, DateTimeKind.Utc) },
        new object[] { new DateTime(2022, 9, 10, 23, 59, 59, DateTimeKind.Utc),  new DateTime(2022, 9, 30, 0, 0, 0, DateTimeKind.Utc) },
        new object[] { new DateTime(2022, 9, 10, 0, 0, 0, DateTimeKind.Utc),  new DateTime(2022, 9, 30, 23, 59, 59, DateTimeKind.Utc) },
        new object[] { new DateTime(2022, 11, 1, 0, 0, 0, DateTimeKind.Utc),   new DateTime(2022, 11, 25, 0, 0, 0, DateTimeKind.Utc) },
        new object[] { new DateTime(2022, 11, 1, 23, 59, 59, DateTimeKind.Utc),   new DateTime(2022, 11, 25, 0, 0, 0, DateTimeKind.Utc) },
        new object[] { new DateTime(2022, 11, 1, 0, 0, 0, DateTimeKind.Utc),   new DateTime(2022, 11, 25, 23, 59, 59, DateTimeKind.Utc) }
    };

    [Theory]
    [MemberData(nameof(Should_return_all_places_given_no_booking_conflict_date_range_data))]
    public async Task Should_return_all_places_given_no_booking_conflict_date_range(DateTime checkIn, DateTime checkOut)
    {
        var query = new SearchAvailablePlacesForBookingQuery(checkIn, checkOut);

        var result = await _mediator.Send(query);

        result.TotalCount.ShouldBe(PLACES.Count());
        var list = await result.Items.ToListAsync();
        foreach(var place in PLACES)
            list.ShouldContain(q => q.Id == place.Id);
    }

    public static List<object[]> Should_return_available_places_given_booking_conflict_date_range_on_some_places_data = new()
    {
        new object[] { new DateTime(2022, 9, 20, 0, 0, 0, DateTimeKind.Utc),  new DateTime(2022, 10, 5, 0, 0, 0, DateTimeKind.Utc), new List<Guid>() { new("2f9aae96-1026-4d76-b8ee-431eb2d32687") } },
        new object[] { new DateTime(2022, 9, 20, 23, 59, 59, DateTimeKind.Utc),  new DateTime(2022, 10, 5, 0, 0, 0, DateTimeKind.Utc), new List<Guid>() { new("2f9aae96-1026-4d76-b8ee-431eb2d32687") } },
        new object[] { new DateTime(2022, 9, 20, 0, 0, 0, DateTimeKind.Utc),  new DateTime(2022, 10, 5, 23, 59, 59, DateTimeKind.Utc), new List<Guid>() { new("2f9aae96-1026-4d76-b8ee-431eb2d32687") } },
        new object[] { new DateTime(2022, 9, 20, 23, 59, 59, DateTimeKind.Utc),  new DateTime(2022, 10, 5, 23, 59, 59, DateTimeKind.Utc), new List<Guid>() { new("2f9aae96-1026-4d76-b8ee-431eb2d32687") } },

        new object[] { new DateTime(2022, 10, 22, 0, 0, 0, DateTimeKind.Utc),  new DateTime(2022, 11, 5, 0, 0, 0, DateTimeKind.Utc), new List<Guid>() { new("fb2709c2-13a4-4559-83cd-a340f3e53e0b"), new("de2b05b4-de6c-4e4c-9ffb-6361be0e7425") } },
        new object[] { new DateTime(2022, 10, 22, 23, 59, 59, DateTimeKind.Utc),  new DateTime(2022, 11, 5, 0, 0, 0, DateTimeKind.Utc), new List<Guid>() { new("fb2709c2-13a4-4559-83cd-a340f3e53e0b"), new("de2b05b4-de6c-4e4c-9ffb-6361be0e7425") } },
        new object[] { new DateTime(2022, 10, 22, 0, 0, 0, DateTimeKind.Utc),  new DateTime(2022, 11, 5, 23, 59, 59, DateTimeKind.Utc), new List<Guid>() { new("fb2709c2-13a4-4559-83cd-a340f3e53e0b"), new("de2b05b4-de6c-4e4c-9ffb-6361be0e7425") } },
        new object[] { new DateTime(2022, 10, 22, 23, 59, 59, DateTimeKind.Utc),  new DateTime(2022, 11, 5, 23, 59, 59, DateTimeKind.Utc), new List<Guid>() { new("fb2709c2-13a4-4559-83cd-a340f3e53e0b"), new("de2b05b4-de6c-4e4c-9ffb-6361be0e7425") } }
    };
    [Theory]
    [MemberData(nameof(Should_return_available_places_given_booking_conflict_date_range_on_some_places_data))]
    public async Task Should_return_available_places_given_booking_conflict_date_range_on_some_places(DateTime checkIn, DateTime checkOut, List<Guid> matches)
    {
        var query = new SearchAvailablePlacesForBookingQuery(checkIn, checkOut);
        var result = await _mediator.Send(query);
        var items = await result.Items.ToListAsync();

        result.TotalCount.ShouldBe(matches.Count());
        foreach (var match in matches)
            items.ShouldContain(q => q.Id == match);
    }

    public static List<object[]> Should_return_no_places_given_booking_conflict_date_range_on_all_places_data = new()
    {
        new object[] { new DateTime(2022, 9, 15, 0, 0, 0, DateTimeKind.Utc),  new DateTime(2022, 11, 15, 0, 0, 0, DateTimeKind.Utc) },
        new object[] { new DateTime(2022, 9, 15, 23, 59, 59, DateTimeKind.Utc),  new DateTime(2022, 11, 15, 0, 0, 0, DateTimeKind.Utc) },
        new object[] { new DateTime(2022, 9, 15, 0, 0, 0, DateTimeKind.Utc),  new DateTime(2022, 11, 15, 23, 59, 59, DateTimeKind.Utc) },
        new object[] { new DateTime(2022, 9, 15, 0, 0, 0, DateTimeKind.Utc),  new DateTime(2022, 10, 20, 0, 0, 0, DateTimeKind.Utc)  },
        new object[] { new DateTime(2022, 9, 15, 23, 59, 59, DateTimeKind.Utc),  new DateTime(2022, 10, 20, 0, 0, 0, DateTimeKind.Utc)  },
        new object[] { new DateTime(2022, 9, 15, 0, 0, 0, DateTimeKind.Utc),  new DateTime(2022, 10, 20, 23, 59, 59, DateTimeKind.Utc)  },
        new object[] { new DateTime(2022, 10, 11, 0, 0, 0, DateTimeKind.Utc),  new DateTime(2022, 11, 15, 0, 0, 0, DateTimeKind.Utc)  },
        new object[] { new DateTime(2022, 10, 11, 23, 59, 59, DateTimeKind.Utc),  new DateTime(2022, 11, 15, 0, 0, 0, DateTimeKind.Utc)  },
        new object[] { new DateTime(2022, 10, 11, 0, 0, 0, DateTimeKind.Utc),  new DateTime(2022, 11, 15, 23, 59, 59, DateTimeKind.Utc)  }
    };
    [Theory]
    [MemberData(nameof(Should_return_no_places_given_booking_conflict_date_range_on_all_places_data))]
    public async Task Should_return_no_places_given_booking_conflict_date_range_on_all_places(DateTime checkIn, DateTime checkOut)
    {
        var query = new SearchAvailablePlacesForBookingQuery(checkIn, checkOut);
        var result = await _mediator.Send(query);
        result.TotalCount.ShouldBe(0);
    }


    public static List<object[]> Should_not_consider_not_confirmed_bookings_given_booking_conflict_date_range_data = new()
    {
        new object[] { new DateTime(2023, 9, 20, 0, 0, 0, DateTimeKind.Utc),  new DateTime(2023, 10, 5, 0, 0, 0, DateTimeKind.Utc) },
        new object[] { new DateTime(2023, 9, 20, 23, 59, 59, DateTimeKind.Utc),  new DateTime(2023, 10, 5, 0, 0, 0, DateTimeKind.Utc) },
        new object[] { new DateTime(2023, 9, 20, 0, 0, 0, DateTimeKind.Utc),  new DateTime(2023, 10, 5, 23, 59, 59, DateTimeKind.Utc) },
        new object[] { new DateTime(2023, 9, 20, 23, 59, 59, DateTimeKind.Utc),  new DateTime(2023, 10, 5, 23, 59, 59, DateTimeKind.Utc) },

        new object[] { new DateTime(2023, 10, 22, 0, 0, 0, DateTimeKind.Utc),  new DateTime(2023, 11, 5, 0, 0, 0, DateTimeKind.Utc) },
        new object[] { new DateTime(2023, 10, 22, 23, 59, 59, DateTimeKind.Utc),  new DateTime(2023, 11, 5, 0, 0, 0, DateTimeKind.Utc) },
        new object[] { new DateTime(2023, 10, 22, 0, 0, 0, DateTimeKind.Utc),  new DateTime(2023, 11, 5, 23, 59, 59, DateTimeKind.Utc) },
        new object[] { new DateTime(2023, 10, 22, 23, 59, 59, DateTimeKind.Utc),  new DateTime(2023, 11, 5, 23, 59, 59, DateTimeKind.Utc) }
    };
    [Theory]
    [MemberData(nameof(Should_not_consider_not_confirmed_bookings_given_booking_conflict_date_range_data))]
    public async Task Should_not_consider_not_confirmed_bookings_given_booking_conflict_date_range(DateTime checkIn, DateTime checkOut)
    {
        var query = new SearchAvailablePlacesForBookingQuery(checkIn, checkOut);
        var result = await _mediator.Send(query);
        var items = await result.Items.ToListAsync();

        result.TotalCount.ShouldBe(PLACES.Count());
        items.ShouldContain(q => q.Id == new Guid("7fd96d44-b61f-4d1f-bba0-d60ac8488875"));
        items.ShouldContain(q => q.Id == new Guid("fb2709c2-13a4-4559-83cd-a340f3e53e0b"));
        items.ShouldContain(q => q.Id == new Guid("2f9aae96-1026-4d76-b8ee-431eb2d32687"));
        items.ShouldContain(q => q.Id == new Guid("de2b05b4-de6c-4e4c-9ffb-6361be0e7425"));
    }

    [Fact]
    public async Task Should_throw_ValidationException_given_inverse_range_of_date()
    {
        var query = new SearchAvailablePlacesForBookingQuery(new DateTime(2022, 12, 20, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 11, 20, 0, 0, 0, DateTimeKind.Utc));
        await Should.ThrowAsync<ArgumentException>(() => _mediator.Publish(query));
    }

    [Fact]
    public async Task Should_compute_total_count_properly()
    {
        var query = new SearchAvailablePlacesForBookingQuery(new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 2, 0, 0, 0, DateTimeKind.Utc), 1, 0);
        var result = await _mediator.Send(query);
        result.TotalCount.ShouldBe(PLACES.Count());
        var items = await result.Items.ToListAsync();
        items.Count().ShouldBe(1);
    }

    [Fact]
    public async Task Should_compute_pagination_properly()
    {
        var query = new SearchAvailablePlacesForBookingQuery(new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 2, 0, 0, 0, DateTimeKind.Utc), 1, 2);
        var result = await _mediator.Send(query);
        var items = await result.Items.ToListAsync();
        items.ShouldContain(q => q.Id == PLACES[2].Id);
    }

    [Fact]
    public async Task Should_sort_collection_by_place_name_properly()
    {
        var sorted = new List<Place>(PLACES).OrderByDescending(q => q.Name).ToList();
        var query = new SearchAvailablePlacesForBookingQuery(new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 2, 0, 0, 0, DateTimeKind.Utc), null, null, "name desc");
        var result = await _mediator.Send(query);
        var items = await result.Items.ToListAsync();
        items.First().Id.ShouldBe(sorted.First().Id);
        items.Last().Id.ShouldBe(sorted.Last().Id);
    }

    [Fact]
    public async Task Should_sort_collection_by_place_id_properly()
    {
        var sorted = new List<Place>(PLACES).OrderByDescending(q => q.Id).ToList();
        var query = new SearchAvailablePlacesForBookingQuery(new DateTime(2023, 1, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2023, 1, 2, 0, 0, 0, DateTimeKind.Utc), null, null, "id desc");
        var result = await _mediator.Send(query);
        var items = await result.Items.ToListAsync();
        items.First().Id.ShouldBe(sorted.First().Id);
        items.Last().Id.ShouldBe(sorted.Last().Id);
    }
}