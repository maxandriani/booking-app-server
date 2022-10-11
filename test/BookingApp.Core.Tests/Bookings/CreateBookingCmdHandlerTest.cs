using BookingApp.Core.Bookings;
using BookingApp.Core.Bookings.Commands;
using BookingApp.Core.Data;
using BookingApp.Core.Places.Models;
using BookingApp.Core.Tests.Commons;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace BookingApp.Core.Tests.Bookings;

public class CreateBookingCmdHandlerTest : TestBase
{
    public static List<object[]> Should_Normalize_Booking_Description_if_Not_NULL_Data = new() {
        new object[] { new Place(new Guid("ba98e21b-9fdd-45aa-a82d-c6537984deba"), "Jona's House 1", "Jona's Street, n1"), new CreateBookingCmd(new Guid("ba98e21b-9fdd-45aa-a82d-c6537984deba"), new DateTime(2022, 10, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 10, 0, 0, 0, DateTimeKind.Utc), null), null! },
        new object[] { new Place(new Guid("55dba9c6-f61f-49b6-a4c1-3ae0e5b7af3c"), "Jona's House 2", "Jona's Street, n2"), new CreateBookingCmd(new Guid("55dba9c6-f61f-49b6-a4c1-3ae0e5b7af3c"), new DateTime(2022, 10, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 10, 0, 0, 0, DateTimeKind.Utc), ""), "" },
        new object[] { new Place(new Guid("55dba9c6-f61f-49b6-a4c1-3ae0e5b7af3c"), "Jona's House 2", "Jona's Street, n2"), new CreateBookingCmd(new Guid("55dba9c6-f61f-49b6-a4c1-3ae0e5b7af3c"), new DateTime(2022, 10, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 10, 0, 0, 0, DateTimeKind.Utc), "Jonas"), "Jonas" },
        new object[] { new Place(new Guid("55dba9c6-f61f-49b6-a4c1-3ae0e5b7af3c"), "Jona's House 2", "Jona's Street, n2"), new CreateBookingCmd(new Guid("55dba9c6-f61f-49b6-a4c1-3ae0e5b7af3c"), new DateTime(2022, 10, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 10, 0, 0, 0, DateTimeKind.Utc), "   "), "" },
        new object[] { new Place(new Guid("55dba9c6-f61f-49b6-a4c1-3ae0e5b7af3c"), "Jona's House 2", "Jona's Street, n2"), new CreateBookingCmd(new Guid("55dba9c6-f61f-49b6-a4c1-3ae0e5b7af3c"), new DateTime(2022, 10, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 10, 0, 0, 0, DateTimeKind.Utc), " Teste"), "Teste" },
        new object[] { new Place(new Guid("edae9255-66a8-465b-88d0-581c80055205"), "Jona's House 3", "Jona's Street, n3"), new CreateBookingCmd(new Guid("edae9255-66a8-465b-88d0-581c80055205"), new DateTime(2022, 10, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 10, 0, 0, 0, DateTimeKind.Utc), null), null! }
    };

    [Theory]
    [MemberData(nameof(Should_Normalize_Booking_Description_if_Not_NULL_Data))]
    public async Task Should_Normalize_Booking_Description_if_Not_NULL(Place place, CreateBookingCmd cmd, string? description)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<CreateBookingCmdHandler>();
        dbContext.Places.Add(place);
        await dbContext.SaveChangesAsync();

        var result = await handler.Handle(cmd, CancellationToken.None);

        result.Description.ShouldBe(description);
    }

    public static List<object[]> Should_Thrown_ValidationException_When_CheckIn_Is_Higher_Then_CheckOut_Data = new() {
        new object[] { new Place(new Guid("ba98e21b-9fdd-45aa-a82d-c6537984deba"), "Jona's House 1", "Jona's Street, n1"), new CreateBookingCmd(new Guid("ba98e21b-9fdd-45aa-a82d-c6537984deba"), new DateTime(2022, 10, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 10, 23, 59, 59, DateTimeKind.Utc)) },
        new object[] { new Place(new Guid("55dba9c6-f61f-49b6-a4c1-3ae0e5b7af3c"), "Jona's House 2", "Jona's Street, n2"), new CreateBookingCmd(new Guid("55dba9c6-f61f-49b6-a4c1-3ae0e5b7af3c"), new DateTime(2022, 10, 10, 23, 59, 59, DateTimeKind.Utc), new DateTime(2022, 10, 10, 0, 0, 0, DateTimeKind.Utc)) },
        new object[] { new Place(new Guid("55dba9c6-f61f-49b6-a4c1-3ae0e5b7af3c"), "Jona's House 2", "Jona's Street, n2"), new CreateBookingCmd(new Guid("55dba9c6-f61f-49b6-a4c1-3ae0e5b7af3c"), new DateTime(2022, 10, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 1, 0, 0, 0, DateTimeKind.Utc)) },
        new object[] { new Place(new Guid("55dba9c6-f61f-49b6-a4c1-3ae0e5b7af3c"), "Jona's House 2", "Jona's Street, n2"), new CreateBookingCmd(new Guid("55dba9c6-f61f-49b6-a4c1-3ae0e5b7af3c"), new DateTime(2022, 10, 10, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 9, 23, 59, 59, DateTimeKind.Utc)) }
    };

    [Theory]
    [MemberData(nameof(Should_Thrown_ValidationException_When_CheckIn_Is_Higher_Then_CheckOut_Data))]
    public async Task Should_Thrown_ValidationException_When_CheckIn_Is_Higher_Then_CheckOut(Place place, CreateBookingCmd cmd)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<CreateBookingCmdHandler>();
        dbContext.Places.Add(place);
        await dbContext.SaveChangesAsync();

        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    public static List<object[]> Should_Add_a_Set_of_Bookings_Data = new() {
        new object[] { new Place(new Guid("ba98e21b-9fdd-45aa-a82d-c6537984deba"), "Jona's House 1", "Jona's Street, n1"), new CreateBookingCmd(new Guid("ba98e21b-9fdd-45aa-a82d-c6537984deba"), new DateTime(2022, 10, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 10, 0, 0, 0, DateTimeKind.Utc), null) },
        new object[] { new Place(new Guid("55dba9c6-f61f-49b6-a4c1-3ae0e5b7af3c"), "Jona's House 2", "Jona's Street, n2"), new CreateBookingCmd(new Guid("55dba9c6-f61f-49b6-a4c1-3ae0e5b7af3c"), new DateTime(2022, 10, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 10, 0, 0, 0, DateTimeKind.Utc), "Jonas") },
    };

    [Theory]
    [MemberData(nameof(Should_Add_a_Set_of_Bookings_Data))]
    public async Task Should_Add_a_Set_of_Bookings(Place place, CreateBookingCmd cmd)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<CreateBookingCmdHandler>();
        dbContext.Places.Add(place);
        await dbContext.SaveChangesAsync();

        var result = await handler.Handle(cmd, CancellationToken.None);
        var prove = await dbContext.Bookings.FirstAsync(q => q.Id == result.Id);

        prove.PlaceId.ShouldBe(cmd.PlaceId);
        prove.CheckIn.ShouldBe(cmd.CheckIn.Date);
        prove.CheckOut.ShouldBe(cmd.CheckOut.Date);
        prove.Description.ShouldBe(cmd.Description);
    }

    [Fact]
    public async Task Should_Add_Booking_W_Pending_State()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<CreateBookingCmdHandler>();
        var place = new Place(new Guid("ba98e21b-9fdd-45aa-a82d-c6537984deba"), "Jona's House 1", "Jona's Street, n1");
        dbContext.Places.Add(place);
        await dbContext.SaveChangesAsync();

        var cmd = new CreateBookingCmd(place.Id, new DateTime(2022, 10, 1, 0, 0, 0, DateTimeKind.Utc), new DateTime(2022, 10, 10, 0, 0, 0, DateTimeKind.Utc), null);
        var result = await handler.Handle(cmd, CancellationToken.None);
        var prove = await dbContext.Bookings.FirstAsync(q => q.Id == result.Id);

        prove.Status.ShouldBe(Core.Bookings.Models.BookingStatusEnum.Pending);
    }
}