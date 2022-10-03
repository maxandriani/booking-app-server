using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.Data;
using BookingApp.Core.Guests;
using BookingApp.Core.Guests.Models;
using BookingApp.Core.Guests.Queries;
using BookingApp.Core.Tests.Commons;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace BookingApp.Core.Tests.Guests;

public class GetGuestByKeyQueryHandlerTest : TestBase
{
    [Fact]
    public async Task Should_throw_ValidationException_When_Guid_Is_Empty()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<GetGuestByKeyQueryHandler>();

        var cmd = new GetGuestByKeyQuery(Guid.Empty);
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Should_throw_ResourceNotFoundException_When_Guest_Does_Not_Exists()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<GetGuestByKeyQueryHandler>();

        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(new GetGuestByKeyQuery(Guid.NewGuid()), CancellationToken.None));
    }

    public static List<object[]> Should_Get_a_Set_of_Guests_Data = new() {
        new object[] { new Guest { Name = "Jonas 1" } },
        new object[] { new Guest { Name = "Jonas 1" } },
        new object[] { new Guest { Name = "Jonas 2" } }
    };

    [Theory]
    [MemberData(nameof(Should_Get_a_Set_of_Guests_Data))]
    public async Task Should_Get_a_Set_of_Guests(Guest guest)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<GetGuestByKeyQueryHandler>();

        dbContext.Add(guest);
        await dbContext.SaveChangesAsync();

        var cmd = new GetGuestByKeyQuery(guest.Id);
        var result = await handler.Handle(cmd, CancellationToken.None);

        result.Id.ShouldBe(guest.Id);
        result.Name.ShouldBe(guest.Name);
    }
}
