using Booking.Core.Commons.Exceptions;
using Booking.Core.Data;
using Booking.Core.Guests;
using Booking.Core.Guests.Commands;
using Booking.Core.Guests.Models;
using Booking.Core.Guests.Responses;
using Booking.Core.Tests.Commons;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Booking.Core.Tests.Guests;

public class DeleteGuestCmdHandlerTest : BaseTest
{
    [Fact]
    public async Task Should_throw_ResourceNotFoundException_When_Guest_Does_Not_Exists()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<DeleteGuestCmdHandler>();

        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(new DeleteGuestCmd(Guid.NewGuid()), CancellationToken.None));
    }

    [Fact]
    public async Task Should_throw_ArgumentOutOfRangeException_When_Guid_Is_Empty()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<DeleteGuestCmdHandler>();

        var cmd = new DeleteGuestCmd(Guid.Empty);
        await Should.ThrowAsync<ArgumentOutOfRangeException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    public static List<object[]> Should_Successfully_Delete_Some_Guests_Data = new() {
        new object[] { new GuestCreateUpdateBody { Name = "Jonas D1" } },
        new object[] { new GuestCreateUpdateBody { Name = "Jonas D2" } },
        new object[] { new GuestCreateUpdateBody { Name = "Jonas D3" } }
    };

    [Theory]
    [MemberData(nameof(Should_Successfully_Delete_Some_Guests_Data))]
    public async Task Should_Successfully_Delete_Some_Guests(GuestCreateUpdateBody body)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<DeleteGuestCmdHandler>();

        var guest = new Guest();
        body.MapTo(guest);
        dbContext.Guests.Add(guest);
        await dbContext.SaveChangesAsync();

        var cmd = new DeleteGuestCmd(guest.Id);
        await Should.NotThrowAsync(() => handler.Handle(cmd, CancellationToken.None));
        dbContext.Guests.Any(q => q.Id == guest.Id).ShouldBe(false);
    }
}
