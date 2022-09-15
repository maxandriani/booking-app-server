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

public class UpdateGuestCmdHandlerTest : BaseTest
{
    [Fact]
    public async Task Should_throw_ValidationException_When_Request_Body_Is_Null()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdateGuestCmdHandler>();

        await Should.ThrowAsync<ValidationException>(() => handler.Handle(new UpdateGuestCmd(Guid.NewGuid(), " "), CancellationToken.None));
    }

    [Fact]
    public async Task Should_throw_ValidationException_When_Guid_Is_Empty()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdateGuestCmdHandler>();

        var cmd = new UpdateGuestCmd(Guid.Empty, string.Empty);
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Should_throw_ResourceNotFoundException_When_Guest_Does_Not_Exists()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdateGuestCmdHandler>();

        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(new UpdateGuestCmd(Guid.NewGuid(), "Jonas"), CancellationToken.None));
    }

    public static List<object[]> Should_throw_ValidationException_When_Guest_Name_Less_Than_3_Data = new() {
        new object[] { new Guest { Id = new Guid("1ba9afbf-3d59-4e22-a06e-bf295419c604"), Name = "Jonas 1" }, new UpdateGuestCmd(new Guid("1ba9afbf-3d59-4e22-a06e-bf295419c604"), "J") },
        new object[] { new Guest { Id = new Guid("c387849b-a38c-4d55-b2c5-fcaf967ae00c"), Name = "Jonas 2" }, new UpdateGuestCmd(new Guid("c387849b-a38c-4d55-b2c5-fcaf967ae00c"), "Jo") },
        new object[] { new Guest { Id = new Guid("748a49f2-4c24-4272-b679-c8233711dc69"), Name = "Jonas 3" }, new UpdateGuestCmd(new Guid("748a49f2-4c24-4272-b679-c8233711dc69"), "") },
        new object[] { new Guest { Id = new Guid("f91a719a-90d2-413f-9ffe-8aaa197c7fe0"), Name = "Jonas 4" }, new UpdateGuestCmd(new Guid("f91a719a-90d2-413f-9ffe-8aaa197c7fe0"), "   ") },
        new object[] { new Guest { Id = new Guid("ba8a4fbf-df22-4fec-a206-50d07919307c"), Name = "Jonas 5" }, new UpdateGuestCmd(new Guid("ba8a4fbf-df22-4fec-a206-50d07919307c"), "   1") },
        new object[] { new Guest { Id = new Guid("5b5a0d07-6ab0-4982-91cd-88d444a0a14b"), Name = "Jonas 6" }, new UpdateGuestCmd(new Guid("5b5a0d07-6ab0-4982-91cd-88d444a0a14b"), "1   ") }
    };
    
    [Theory]
    [MemberData(nameof(Should_throw_ValidationException_When_Guest_Name_Less_Than_3_Data))]
    public async Task Should_throw_ValidationException_When_Guest_Name_Less_Than_3(Guest guest, UpdateGuestCmd cmd)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdateGuestCmdHandler>();

        dbContext.Add(guest);
        await dbContext.SaveChangesAsync();

        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    public static List<object[]> Should_Update_a_Set_of_Guests_Data = new() {
        new object[] { new Guest { Id = new Guid("ad0eb1ad-8007-4729-a6d7-e6abe4479497"), Name = "Jonas 1" }, new UpdateGuestCmd(new Guid("ad0eb1ad-8007-4729-a6d7-e6abe4479497"), "Jonas 31") },
        new object[] { new Guest { Id = new Guid("8522cf1a-f449-4fcd-85a7-05824c709042"), Name = "Jonas 2" }, new UpdateGuestCmd(new Guid("8522cf1a-f449-4fcd-85a7-05824c709042"), "Jonas 32") },
        new object[] { new Guest { Id = new Guid("75e42e97-1a95-4878-904c-b28c82d7a820"), Name = "Jonas 3" }, new UpdateGuestCmd(new Guid("75e42e97-1a95-4878-904c-b28c82d7a820"), "Jonas 33") },
    };

    [Theory]
    [MemberData(nameof(Should_Update_a_Set_of_Guests_Data))]
    public async Task Should_Update_a_Set_of_Guests(Guest guest, UpdateGuestCmd cmd)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdateGuestCmdHandler>();
        
        dbContext.Add(guest);
        await dbContext.SaveChangesAsync();

        await Should.NotThrowAsync(() => handler.Handle(cmd, CancellationToken.None));
        dbContext.Guests.Any(q => q.Name.Equals(cmd.Name)).ShouldBe(true);
        dbContext.Guests.First(q => q.Id.Equals(guest.Id)).Name.ShouldBe(cmd.Name);
    }
}
