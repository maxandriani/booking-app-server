using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.Data;
using BookingApp.Core.Guests.Queries;
using BookingApp.Core.Places;
using BookingApp.Core.Places.Models;
using BookingApp.Core.Places.Queries;
using BookingApp.Core.Tests.Commons;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace BookingApp.Core.Tests.Places;

public class GetPlaceByKeyQueryHandlerTest : TestBase
{
    [Fact]
    public async Task Should_throw_ValidationException_When_Guid_Is_Empty()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<GetPlaceByKeyQueryHandler>();

        var cmd = new GetPlaceByKeyQuery(Guid.Empty);
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Should_throw_ResourceNotFoundException_When_Place_Does_Not_Exists()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<GetPlaceByKeyQueryHandler>();

        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(new GetPlaceByKeyQuery(Guid.NewGuid()), CancellationToken.None));
    }

    public static List<object[]> Should_Get_a_Set_of_Places_Data = new() {
        new object[] { new Place { Name = "Jonas 1", Address = "Jona's Place 1" } },
        new object[] { new Place { Name = "Jonas 1", Address = "Jona's Place 2" } },
        new object[] { new Place { Name = "Jonas 2", Address = "Jona's Place 3" } }
    };

    [Theory]
    [MemberData(nameof(Should_Get_a_Set_of_Places_Data))]
    public async Task Should_Get_a_Set_of_Places(Place Place)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<GetPlaceByKeyQueryHandler>();

        dbContext.Add(Place);
        await dbContext.SaveChangesAsync();

        var cmd = new GetPlaceByKeyQuery(Place.Id);
        var result = await handler.Handle(cmd, CancellationToken.None);

        result.Id.ShouldBe(Place.Id);
        result.Name.ShouldBe(Place.Name);
    }
}
