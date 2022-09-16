using Booking.Core.Commons.Exceptions;
using Booking.Core.Data;
using Booking.Core.Places;
using Booking.Core.Places.Commands;
using Booking.Core.Tests.Commons;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Booking.Core.Tests.Places;

public class CreatePlaceCmdHandlerTest : BaseTest
{

    public static List<object[]> Should_throw_ValidationException_When_Place_Name_Less_Than_3_Data = new() {
        new object[] { new CreatePlaceCmd("J", "abc") },
        new object[] { new CreatePlaceCmd("Jo", "abc") },
        new object[] { new CreatePlaceCmd("", "abc") },
        new object[] { new CreatePlaceCmd("   ", "abc") },
        new object[] { new CreatePlaceCmd("   1", "abc") },
        new object[] { new CreatePlaceCmd("1   ", "abc") }
    };
    
    [Theory]
    [MemberData(nameof(Should_throw_ValidationException_When_Place_Address_Less_Than_3_Data))]
    public async Task Should_throw_ValidationException_When_Place_Address_Less_Than_3(CreatePlaceCmd cmd)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<CreatePlaceCmdHandler>();

        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    public static List<object[]> Should_throw_ValidationException_When_Place_Address_Less_Than_3_Data = new() {
        new object[] { new CreatePlaceCmd("Jona's House 1", "a") },
        new object[] { new CreatePlaceCmd("Jona's House 2", "ab") },
        new object[] { new CreatePlaceCmd("Jona's House 3", "") },
        new object[] { new CreatePlaceCmd("Jona's House 4", "   ") },
        new object[] { new CreatePlaceCmd("Jona's House 5", "   1") },
        new object[] { new CreatePlaceCmd("Jona's House 6", "1   ") }
    };
    
    [Theory]
    [MemberData(nameof(Should_throw_ValidationException_When_Place_Name_Less_Than_3_Data))]
    public async Task Should_throw_ValidationException_When_Place_Name_Less_Than_3(CreatePlaceCmd cmd)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<CreatePlaceCmdHandler>();

        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    public static List<object[]> Should_Add_a_Set_of_Places_Data = new() {
        new object[] { new CreatePlaceCmd("Jona's Place 1", "Jona's Street, n 2") },
        new object[] { new CreatePlaceCmd("Jona's Place 2", "Jona's Street, n 2") },
        new object[] { new CreatePlaceCmd("Jona's Place 3", "Jona's Street, n 2") }
    };

    [Theory]
    [MemberData(nameof(Should_Add_a_Set_of_Places_Data))]
    public async Task Should_Add_a_Set_of_Places(CreatePlaceCmd cmd)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<CreatePlaceCmdHandler>();

        await Should.NotThrowAsync(() => handler.Handle(cmd, CancellationToken.None));
        dbContext.Places.Any(q => q.Name.Equals(cmd.Name)).ShouldBe(true);
        dbContext.Places.Any(q => q.Address.Equals(cmd.Address)).ShouldBe(true);
    }

    public async Task Should_Not_Add_Duplicated_Place(CreatePlaceCmd cmd)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<CreatePlaceCmdHandler>();

        var place1 = new CreatePlaceCmd("Jona's Place 1", "Jona's Street, n 2");
        var place2 = new CreatePlaceCmd("Jona's Place 1", "Jona's Street, n 234");

        await handler.Handle(place1, CancellationToken.None);
        await Should.ThrowAsync<ResourceAlreadyExistsException>(() => handler.Handle(place2, CancellationToken.None));
    }
}
