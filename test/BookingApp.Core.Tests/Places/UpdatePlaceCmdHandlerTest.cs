using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.Data;
using BookingApp.Core.Places;
using BookingApp.Core.Places.Commands;
using BookingApp.Core.Places.Models;
using BookingApp.Core.Tests.Commons;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace BookingApp.Core.Tests.Places;

public class UpdatePlaceCmdHandlerTest : TestBase
{
    [Fact]
    public async Task Should_throw_ValidationException_When_Guid_Is_Empty()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdatePlaceCmdHandler>();

        var cmd = new UpdatePlaceCmd(Guid.Empty, "Jonas 1", "Jona's Street");
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Should_throw_ResourceNotFoundException_When_Place_Does_Not_Exists()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdatePlaceCmdHandler>();

        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(new UpdatePlaceCmd(Guid.NewGuid(), "Jonas", "Jona's Street"), CancellationToken.None));
    }

    public static List<object[]> Should_throw_ValidationException_When_Place_Name_Less_Than_3_Data = new() {
        new object[] { new Place { Id = new Guid("1ba9afbf-3d59-4e22-a06e-bf295419c604"), Name = "Jonas 1" }, new UpdatePlaceCmd(new Guid("1ba9afbf-3d59-4e22-a06e-bf295419c604"), "J", "Jona's Street 1") },
        new object[] { new Place { Id = new Guid("c387849b-a38c-4d55-b2c5-fcaf967ae00c"), Name = "Jonas 2" }, new UpdatePlaceCmd(new Guid("c387849b-a38c-4d55-b2c5-fcaf967ae00c"), "Jo", "Jona's Street 1") },
        new object[] { new Place { Id = new Guid("748a49f2-4c24-4272-b679-c8233711dc69"), Name = "Jonas 3" }, new UpdatePlaceCmd(new Guid("748a49f2-4c24-4272-b679-c8233711dc69"), "", "Jona's Street 1") },
        new object[] { new Place { Id = new Guid("f91a719a-90d2-413f-9ffe-8aaa197c7fe0"), Name = "Jonas 4" }, new UpdatePlaceCmd(new Guid("f91a719a-90d2-413f-9ffe-8aaa197c7fe0"), "   ", "Jona's Street 1") },
        new object[] { new Place { Id = new Guid("ba8a4fbf-df22-4fec-a206-50d07919307c"), Name = "Jonas 5" }, new UpdatePlaceCmd(new Guid("ba8a4fbf-df22-4fec-a206-50d07919307c"), "   1", "Jona's Street 1") },
        new object[] { new Place { Id = new Guid("5b5a0d07-6ab0-4982-91cd-88d444a0a14b"), Name = "Jonas 6" }, new UpdatePlaceCmd(new Guid("5b5a0d07-6ab0-4982-91cd-88d444a0a14b"), "1   ", "Jona's Street 1") }
    };

    [Theory]
    [MemberData(nameof(Should_throw_ValidationException_When_Place_Name_Less_Than_3_Data))]
    public async Task Should_throw_ValidationException_When_Place_Name_Less_Than_3(Place Place, UpdatePlaceCmd cmd)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdatePlaceCmdHandler>();

        dbContext.Add(Place);
        await dbContext.SaveChangesAsync();

        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    public static List<object[]> Should_throw_ValidationException_When_Place_Address_Less_Than_3_Data = new() {
        new object[] { new Place { Id = new Guid("1ba9afbf-3d59-4e22-a06e-bf295419c604"), Name = "Jonas 1" }, new UpdatePlaceCmd(new Guid("1ba9afbf-3d59-4e22-a06e-bf295419c604"), "Jona's Street 1", "J") },
        new object[] { new Place { Id = new Guid("c387849b-a38c-4d55-b2c5-fcaf967ae00c"), Name = "Jonas 2" }, new UpdatePlaceCmd(new Guid("c387849b-a38c-4d55-b2c5-fcaf967ae00c"), "Jona's Street 1", "Jo") },
        new object[] { new Place { Id = new Guid("748a49f2-4c24-4272-b679-c8233711dc69"), Name = "Jonas 3" }, new UpdatePlaceCmd(new Guid("748a49f2-4c24-4272-b679-c8233711dc69"), "Jona's Street 1", "") },
        new object[] { new Place { Id = new Guid("f91a719a-90d2-413f-9ffe-8aaa197c7fe0"), Name = "Jonas 4" }, new UpdatePlaceCmd(new Guid("f91a719a-90d2-413f-9ffe-8aaa197c7fe0"), "Jona's Street 1", "   ") },
        new object[] { new Place { Id = new Guid("ba8a4fbf-df22-4fec-a206-50d07919307c"), Name = "Jonas 5" }, new UpdatePlaceCmd(new Guid("ba8a4fbf-df22-4fec-a206-50d07919307c"), "Jona's Street 1", "   1") },
        new object[] { new Place { Id = new Guid("5b5a0d07-6ab0-4982-91cd-88d444a0a14b"), Name = "Jonas 6" }, new UpdatePlaceCmd(new Guid("5b5a0d07-6ab0-4982-91cd-88d444a0a14b"), "Jona's Street 1", "1   ") }
    };

    [Theory]
    [MemberData(nameof(Should_throw_ValidationException_When_Place_Address_Less_Than_3_Data))]
    public async Task Should_throw_ValidationException_When_Place_Address_Less_Than_3(Place Place, UpdatePlaceCmd cmd)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdatePlaceCmdHandler>();

        dbContext.Add(Place);
        await dbContext.SaveChangesAsync();

        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    public static List<object[]> Should_Update_a_Set_of_Places_Data = new() {
        new object[] { new Place { Id = new Guid("ad0eb1ad-8007-4729-a6d7-e6abe4479497"), Name = "Jonas 1", Address = "Jona's Not Place" }, new UpdatePlaceCmd(new Guid("ad0eb1ad-8007-4729-a6d7-e6abe4479497"), "Jonas 31", "Jona's Place") },
        new object[] { new Place { Id = new Guid("8522cf1a-f449-4fcd-85a7-05824c709042"), Name = "Jonas 2", Address = "Jona's Not Place" }, new UpdatePlaceCmd(new Guid("8522cf1a-f449-4fcd-85a7-05824c709042"), "Jonas 32", "Jona's Place") },
        new object[] { new Place { Id = new Guid("75e42e97-1a95-4878-904c-b28c82d7a820"), Name = "Jonas 3", Address = "Jona's Not Place" }, new UpdatePlaceCmd(new Guid("75e42e97-1a95-4878-904c-b28c82d7a820"), "Jonas 33", "Jona's Place") },
    };

    [Theory]
    [MemberData(nameof(Should_Update_a_Set_of_Places_Data))]
    public async Task Should_Update_a_Set_of_Places(Place Place, UpdatePlaceCmd cmd)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdatePlaceCmdHandler>();

        dbContext.Add(Place);
        await dbContext.SaveChangesAsync();

        await Should.NotThrowAsync(() => handler.Handle(cmd, CancellationToken.None));
        dbContext.Places.Any(q => q.Name.Equals(cmd.Name)).ShouldBe(true);
        dbContext.Places.First(q => q.Id.Equals(Place.Id)).Name.ShouldBe(cmd.Name);
        dbContext.Places.First(q => q.Id.Equals(Place.Id)).Address.ShouldBe(cmd.Address);
    }

    public async Task Should_Not_Update_Duplicated_Place(CreatePlaceCmd cmd)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdatePlaceCmdHandler>();

        var place1 = new Place(new Guid("8beb277a-8001-46e8-a475-0590947181c4"), "Jona's Place 1", "Jona's Street, n 1");
        var place2 = new Place(new Guid("4aa801d2-52a7-425c-9eca-61a489f5b09f"), "Jona's Place 2", "Jona's Street, n 2");
        var placeCmd = new UpdatePlaceCmd(new Guid("4aa801d2-52a7-425c-9eca-61a489f5b09f"), "Jona's Place 1", "Jona's Street, n 234");

        dbContext.AddRange(place1, place2);
        await dbContext.SaveChangesAsync();

        await Should.ThrowAsync<ResourceAlreadyExistsException>(() => handler.Handle(placeCmd, CancellationToken.None));
    }
}
