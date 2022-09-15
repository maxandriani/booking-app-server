using Booking.Core.Commons.Exceptions;
using Booking.Core.Data;
using Booking.Core.Guests;
using Booking.Core.Guests.Commands;
using Booking.Core.Guests.Models;
using Booking.Core.Tests.Commons;
using FluentValidation;
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
    public async Task Should_throw_ValidationException_When_Guid_Is_Empty()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<DeleteGuestCmdHandler>();

        var cmd = new DeleteGuestCmd(Guid.Empty);
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    public static List<object[]> Should_Successfully_Delete_Some_Guests_Data = new() {
        new object[] { new Guest { Name = "Jonas D1" } },
        new object[] { new Guest { Name = "Jonas D2" } },
        new object[] { new Guest { Name = "Jonas D3" } }
    };

    [Theory]
    [MemberData(nameof(Should_Successfully_Delete_Some_Guests_Data))]
    public async Task Should_Successfully_Delete_Some_Guests(Guest guest)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<DeleteGuestCmdHandler>();

        dbContext.Guests.Add(guest);
        await dbContext.SaveChangesAsync();

        var cmd = new DeleteGuestCmd(guest.Id);
        await Should.NotThrowAsync(() => handler.Handle(cmd, CancellationToken.None));
        dbContext.Guests.Any(q => q.Id == guest.Id).ShouldBe(false);
    }
}
