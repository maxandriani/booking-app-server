using Booking.Core.Commons.Exceptions;
using Booking.Core.Data;
using Booking.Core.Places;
using Booking.Core.Places.Commands;
using Booking.Core.Places.Models;
using Booking.Core.Tests.Commons;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Booking.Core.Tests.Places;

public class DeletePlaceCmdHandlerTest : BaseTest
{
    [Fact]
    public async Task Should_throw_ResourceNotFoundException_When_Place_Does_Not_Exists()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<DeletePlaceCmdHandler>();

        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(new DeletePlaceCmd(Guid.NewGuid()), CancellationToken.None));
    }

    [Fact]
    public async Task Should_throw_ValidationException_When_Guid_Is_Empty()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<DeletePlaceCmdHandler>();

        var cmd = new DeletePlaceCmd(Guid.Empty);
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    public static List<object[]> Should_Successfully_Delete_Some_Places_Data = new() {
        new object[] { new Place { Name = "Jonas D1", Address = "Jonas's Street 1" } },
        new object[] { new Place { Name = "Jonas D2", Address = "Jonas's Street 1" } },
        new object[] { new Place { Name = "Jonas D3", Address = "Jonas's Street 1" } }
    };

    [Theory]
    [MemberData(nameof(Should_Successfully_Delete_Some_Places_Data))]
    public async Task Should_Successfully_Delete_Some_Places(Place Place)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<DeletePlaceCmdHandler>();

        dbContext.Places.Add(Place);
        await dbContext.SaveChangesAsync();

        var cmd = new DeletePlaceCmd(Place.Id);
        await Should.NotThrowAsync(() => handler.Handle(cmd, CancellationToken.None));
        dbContext.Places.Any(q => q.Id == Place.Id).ShouldBe(false);
    }
}
