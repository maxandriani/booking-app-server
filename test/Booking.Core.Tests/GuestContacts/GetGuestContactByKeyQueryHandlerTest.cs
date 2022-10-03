using Booking.Core.Commons.Exceptions;
using Booking.Core.Data;
using Booking.Core.GuestContacts;
using Booking.Core.GuestContacts.Models;
using Booking.Core.GuestContacts.Queries;
using Booking.Core.Guests.Models;
using Booking.Core.Tests.Commons;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Booking.Core.Tests.GuestContacts;

public class GetGuestContactByKeyQueryHandlerTest : TestBase
{
    [Fact]
    public async Task Should_throw_ValidationException_When_Id_Is_Empty()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<GetGuestContactByKeyQueryHandler>();
        var guest = new Guest(new Guid("134d95ff-c46a-4034-9cfb-84e3a54a4cbd"), "Test 4");

        dbContext.Add(guest);
        await dbContext.SaveChangesAsync();

        var cmd = new GetGuestContactByKeyQuery(Guid.Empty, guest.Id);
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Should_throw_ValidationException_When_GuestId_Is_Empty()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<GetGuestContactByKeyQueryHandler>();

        var cmd = new GetGuestContactByKeyQuery(new Guid("a308b88b-067d-478b-876f-b3d954b60607"), Guid.Empty);
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Should_throw_ResourceNotFoundException_When_Guest_Does_Not_Exists()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<GetGuestContactByKeyQueryHandler>();
        
        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(
            new GetGuestContactByKeyQuery(Guid.NewGuid(), new Guid("ac077827-1d8a-4230-8f07-ecf12f8128d3")), CancellationToken.None));
    }

    [Fact]
    public async Task Should_throw_ResourceNotFoundException_When_GuestContact_Does_Not_Exists()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<GetGuestContactByKeyQueryHandler>();
        var guest = new Guest(new Guid("4377e75e-97e2-4437-9de1-88f4fe7d8a8f"), "Host 1");

        dbContext.Add(guest);
        await dbContext.SaveChangesAsync();

        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(
            new GetGuestContactByKeyQuery(Guid.NewGuid(), guest.Id), CancellationToken.None));
    }

    public static List<object[]> Should_Get_a_Set_of_GuestContacts_Data = new() {
        new object[] { new Guest(new Guid("ff2f8341-7d38-4e71-b9dc-0e2d3a97bc13"), "Jonas 1", new List<GuestContact>() { new GuestContact(new Guid("df6767ae-b83c-4757-92c4-2c1ebe1e65d1"), Guid.Empty, GuestContactTypeEnum.Email, "Test Contact 1") }.AsReadOnly()) },
        new object[] { new Guest(new Guid("4295c364-8418-44be-b352-9f344a97f302"), "Jonas 1", new List<GuestContact>() { new GuestContact(new Guid("b170ca25-60ae-4169-ae8b-40aa6ca65fe3"), Guid.Empty, GuestContactTypeEnum.Email, "Test Contact 2") }.AsReadOnly()) },
        new object[] { new Guest(new Guid("636cfdc9-a280-4380-96d0-223ecff6d96c"), "Jonas 2", new List<GuestContact>() { new GuestContact(new Guid("18262f55-d5c2-432c-8672-eac0df3f336e"), Guid.Empty, GuestContactTypeEnum.Email, "Test Contact 3") }.AsReadOnly()) }
    };

    [Theory]
    [MemberData(nameof(Should_Get_a_Set_of_GuestContacts_Data))]
    public async Task Should_Get_a_Set_of_GuestContacts(Guest guest)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<GetGuestContactByKeyQueryHandler>();

        dbContext.Add(guest);
        await dbContext.SaveChangesAsync();
        var contact = guest.Contacts.First();

        var cmd = new GetGuestContactByKeyQuery(contact.Id, guest.Id);
        var result = await handler.Handle(cmd, CancellationToken.None);

        result.Id.ShouldBe(contact.Id);
        result.GuestId.ShouldBe(guest.Id);
        result.Type.ShouldBe(contact.Type);
        result.Value.ShouldBe(contact.Value);
    }
}
