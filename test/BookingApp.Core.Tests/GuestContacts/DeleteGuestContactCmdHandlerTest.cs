using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.Data;
using BookingApp.Core.GuestContacts.Commands;
using BookingApp.Core.Tests.Commons;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;
using BookingApp.Core.Guests.Models;
using BookingApp.Core.GuestContacts;
using BookingApp.Core.GuestContacts.Models;
using FluentValidation;

namespace BookingApp.Core.Tests.GuestContacts;

public class DeleteGuestContactCmdHandlerTest : TestBase
{
    [Fact]
    public async Task Should_throw_ResourceNotFoundException_When_Guest_Does_Not_Exists()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<DeleteGuestContactCmdHandler>();

        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(new DeleteGuestContactCmd(Guid.NewGuid(), Guid.NewGuid()), CancellationToken.None));
    }

    [Fact]
    public async Task Should_throw_ResourceNotFoundException_When_GuestContact_Does_Not_Exists()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<DeleteGuestContactCmdHandler>();
        var guest = new Guest(new Guid("820a3d03-e9fe-4faa-86f8-9cd83c1ba35b"), "Guest Name");

        dbContext.Add(guest);
        await dbContext.SaveChangesAsync();

        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(new DeleteGuestContactCmd(guest.Id, Guid.NewGuid()), CancellationToken.None));
    }

    [Fact]
    public async Task Should_throw_ValidationException_When_GuidId_Is_Empty()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<DeleteGuestContactCmdHandler>();

        var cmd = new DeleteGuestContactCmd(Guid.Empty, new Guid("c24dd681-1b44-4eec-9ee2-a43a40d9dada"));
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Should_throw_ValidationException_When_Id_Is_Empty()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<DeleteGuestContactCmdHandler>();
        var guest = new Guest(new Guid("a77cb9bd-bf9a-4190-ba01-1bbfcd3e8645"), "Guest Name");

        dbContext.Add(guest);
        await dbContext.SaveChangesAsync();

        var cmd = new DeleteGuestContactCmd(guest.Id, Guid.Empty);
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    public static List<object[]> Should_Successfully_Delete_Some_Guests_Data = new() {
        new object[] { new Guest(new Guid("42f45b63-958c-4817-9f65-3254c2035fbb"), "Test Guest", new List<GuestContact> { new GuestContact(new Guid("8207ba4a-d3a1-436c-b46d-707808ff86ab"), new Guid("42f45b63-958c-4817-9f65-3254c2035fbb"), GuestContactTypeEnum.Email, "Contact value") }.AsReadOnly()), new DeleteGuestContactCmd(new Guid("8207ba4a-d3a1-436c-b46d-707808ff86ab"), new Guid("42f45b63-958c-4817-9f65-3254c2035fbb")) },
        new object[] { new Guest(new Guid("e3d06a6c-2335-475b-bb56-a17c81ebf035"), "Test Guest", new List<GuestContact> { new GuestContact(new Guid("7cec042f-17bc-4386-8bcd-177a81fb389f"), new Guid("e3d06a6c-2335-475b-bb56-a17c81ebf035"), GuestContactTypeEnum.Email, "Contact value") }.AsReadOnly()), new DeleteGuestContactCmd(new Guid("7cec042f-17bc-4386-8bcd-177a81fb389f"), new Guid("e3d06a6c-2335-475b-bb56-a17c81ebf035")) },
        new object[] { new Guest(new Guid("2d28ddf5-5460-4b81-9e04-cde16891b1af"), "Test Guest", new List<GuestContact> { new GuestContact(new Guid("c1f013eb-fa00-40e7-8592-619a78e27115"), new Guid("2d28ddf5-5460-4b81-9e04-cde16891b1af"), GuestContactTypeEnum.Email, "Contact value") }.AsReadOnly()), new DeleteGuestContactCmd(new Guid("c1f013eb-fa00-40e7-8592-619a78e27115"), new Guid("2d28ddf5-5460-4b81-9e04-cde16891b1af")) }
    };

    [Theory]
    [MemberData(nameof(Should_Successfully_Delete_Some_Guests_Data))]
    public async Task Should_Successfully_Delete_Some_Guests(Guest guest, DeleteGuestContactCmd cmd)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<DeleteGuestContactCmdHandler>();

        dbContext.Guests.Add(guest);
        await dbContext.SaveChangesAsync();

        await Should.NotThrowAsync(() => handler.Handle(cmd, CancellationToken.None));
        dbContext.GuestContacts.Any(q => q.GuestId == guest.Id && q.Id == cmd.Id).ShouldBe(false);
    }
}
