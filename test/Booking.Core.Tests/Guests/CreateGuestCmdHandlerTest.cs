using Booking.Core.Data;
using Booking.Core.Guests;
using Booking.Core.Guests.Commands;
using Booking.Core.Guests.Responses;
using Booking.Core.Tests.Commons;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Booking.Core.Tests.Guests;

public class CreateGuestCmdHandlerTest : BaseTest
{

    [Fact]
    public async Task Should_throw_ArgumentNullException_When_Request_Body_Is_Null()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<CreateGuestCmdHandler>();

        await Should.ThrowAsync<ArgumentNullException>(() => handler.Handle(new CreateGuestCmd(null), CancellationToken.None));
    }

    public static List<object[]> Should_throw_ValidationException_When_Guest_Name_Less_Than_3_Data = new() {
        new object[] { new GuestCreateUpdateBody { Name = "J" } },
        new object[] { new GuestCreateUpdateBody { Name = "Jo" } },
        new object[] { new GuestCreateUpdateBody { Name = "" } },
        new object[] { new GuestCreateUpdateBody { Name = "   " } },
        new object[] { new GuestCreateUpdateBody { Name = "   1" } },
        new object[] { new GuestCreateUpdateBody { Name = "1   " } }
    };
    
    [Theory]
    [MemberData(nameof(Should_throw_ValidationException_When_Guest_Name_Less_Than_3_Data))]
    public async Task Should_throw_ValidationException_When_Guest_Name_Less_Than_3(GuestCreateUpdateBody body)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<CreateGuestCmdHandler>();

        var cmd = new CreateGuestCmd(body);
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    public static List<object[]> Should_Add_a_Set_of_Guests_Data = new() {
        new object[] { new GuestCreateUpdateBody { Name = "Jonas 1" } },
        new object[] { new GuestCreateUpdateBody { Name = "Jonas 1" } },
        new object[] { new GuestCreateUpdateBody { Name = "Jonas 2" } }
    };

    [Theory]
    [MemberData(nameof(Should_Add_a_Set_of_Guests_Data))]
    public async Task Should_Add_a_Set_of_Guests(GuestCreateUpdateBody body)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<CreateGuestCmdHandler>();

        var cmd = new CreateGuestCmd(body);
        await Should.NotThrowAsync(() => handler.Handle(cmd, CancellationToken.None));
        dbContext.Guests.Any(q => q.Name.Equals(body.Name)).ShouldBe(true);
    }
}
