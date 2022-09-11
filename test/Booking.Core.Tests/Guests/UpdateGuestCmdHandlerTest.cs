using Booking.Core.Commons.Exceptions;
using Booking.Core.Data;
using Booking.Core.Guests;
using Booking.Core.Guests.Commands;
using Booking.Core.Guests.Models;
using Booking.Core.Guests.Responses;
using Booking.Core.Tests.Commons;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Booking.Core.Tests.Guests;

public class UpdateGuestCmdHandlerTest : BaseTest
{
    [Fact]
    public async Task Should_throw_ArgumentNullException_When_Request_Body_Is_Null()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdateGuestCmdHandler>();

        await Should.ThrowAsync<ArgumentNullException>(() => handler.Handle(new UpdateGuestCmd(Guid.NewGuid(), null), CancellationToken.None));
    }

    [Fact]
    public async Task Should_throw_ArgumentOutOfRangeException_When_Guid_Is_Empty()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdateGuestCmdHandler>();

        var cmd = new UpdateGuestCmd(Guid.Empty, new GuestCreateUpdateBody());
        await Should.ThrowAsync<ArgumentOutOfRangeException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Should_throw_ResourceNotFoundException_When_Guest_Does_Not_Exists()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdateGuestCmdHandler>();

        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(new UpdateGuestCmd(Guid.NewGuid(), new GuestCreateUpdateBody()), CancellationToken.None));
    }

    public static List<object[]> Should_throw_ValidationException_When_Guest_Name_Less_Than_3_Data = new() {
        new object[] { new Guest { Name = "Jonas 1" }, new GuestCreateUpdateBody { Name = "J" } },
        new object[] { new Guest { Name = "Jonas 2" }, new GuestCreateUpdateBody { Name = "Jo" } },
        new object[] { new Guest { Name = "Jonas 3" }, new GuestCreateUpdateBody { Name = "" } },
        new object[] { new Guest { Name = "Jonas 4" }, new GuestCreateUpdateBody { Name = "   " } },
        new object[] { new Guest { Name = "Jonas 5" }, new GuestCreateUpdateBody { Name = "   1" } },
        new object[] { new Guest { Name = "Jonas 6" }, new GuestCreateUpdateBody { Name = "1   " } }
    };
    
    [Theory]
    [MemberData(nameof(Should_throw_ValidationException_When_Guest_Name_Less_Than_3_Data))]
    public async Task Should_throw_ValidationException_When_Guest_Name_Less_Than_3(Guest guest, GuestCreateUpdateBody body)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdateGuestCmdHandler>();

        dbContext.Add(guest);
        await dbContext.SaveChangesAsync();

        var cmd = new UpdateGuestCmd(guest.Id, body);
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    public static List<object[]> Should_Update_a_Set_of_Guests_Data = new() {
        new object[] { new Guest { Name = "Jonas 1" }, new GuestCreateUpdateBody { Name = "Jonas 31" } },
        new object[] { new Guest { Name = "Jonas 2" }, new GuestCreateUpdateBody { Name = "Jonas 32" } },
        new object[] { new Guest { Name = "Jonas 3" }, new GuestCreateUpdateBody { Name = "Jonas 33" } },
    };

    [Theory]
    [MemberData(nameof(Should_Update_a_Set_of_Guests_Data))]
    public async Task Should_Update_a_Set_of_Guests(Guest guest, GuestCreateUpdateBody body)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdateGuestCmdHandler>();
        
        dbContext.Add(guest);
        await dbContext.SaveChangesAsync();

        var cmd = new UpdateGuestCmd(guest.Id, body);
        await Should.NotThrowAsync(() => handler.Handle(cmd, CancellationToken.None));
        dbContext.Guests.Any(q => q.Name.Equals(body.Name)).ShouldBe(true);
        dbContext.Guests.First(q => q.Id.Equals(guest.Id)).Name.ShouldBe(body.Name);
    }
}
