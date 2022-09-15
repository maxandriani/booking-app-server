using Booking.Core.Commons.Exceptions;
using Booking.Core.Data;
using Booking.Core.GuestContacts;
using Booking.Core.GuestContacts.Commands;
using Booking.Core.GuestContacts.Models;
using Booking.Core.Guests.Models;
using Booking.Core.Tests.Commons;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Booking.Core.Tests.Guests;

public class UpdateGuestContactCmdHandlerTest : BaseTest
{

    [Fact]
    public async Task Should_throw_ValidationException_When_Guid_Is_Empty()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdateGuestContactCmdHandler>();
        var guest = new Guest(new Guid("2153137f-723c-4469-9853-47c57f1d3210"), "Teste");

        dbContext.Add(guest);
        await dbContext.SaveChangesAsync();

        var cmd = new UpdateGuestContactCmd(Guid.Empty, guest.Id, GuestContactTypeEnum.Email, "Teste");
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Should_throw_ValidationException_When_GuestId_Is_Empty()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdateGuestContactCmdHandler>();
        
        var cmd = new UpdateGuestContactCmd(new Guid("2153137f-723c-4469-9853-47c57f1d3210"), Guid.Empty, GuestContactTypeEnum.Email, "Teste");
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    [Fact]
    public async Task Should_throw_ResourceNotFoundException_When_Guest_Does_Not_Exists()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdateGuestContactCmdHandler>();

        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(new UpdateGuestContactCmd(Guid.NewGuid(), new Guid("f6be7fb6-b3aa-46b3-81c7-47567a618610"), GuestContactTypeEnum.Undefined, "as"), CancellationToken.None));
    }

    [Fact]
    public async Task Should_throw_ResourceNotFoundException_When_GuestContact_Does_Not_Exists()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdateGuestContactCmdHandler>();
        var guest = new Guest(new Guid("9c7b45ff-a6a6-4700-a757-9c26ad924a89"), "Teste");

        dbContext.Add(guest);
        await dbContext.SaveChangesAsync();

        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(new UpdateGuestContactCmd(Guid.NewGuid(), guest.Id, GuestContactTypeEnum.Undefined, "Teste"), CancellationToken.None));
    }

    public static List<object[]> Should_throw_ValidationException_When_Value_is_Empty_Data = new() {
        new object[] { new Guest(new Guid("810f4420-834f-4cab-b3c7-83e49dd1a874"), "Jonas 3", new List<GuestContact>() { new GuestContact(new Guid("d34cbcc6-ba8e-499a-9b38-1f18ed15a5f5"), new Guid("810f4420-834f-4cab-b3c7-83e49dd1a874"), GuestContactTypeEnum.Email, "Teste 1") }.AsReadOnly()), "" },
        new object[] { new Guest(new Guid("98b6ffbc-c624-4816-a34e-fa7fbca546f9"), "Jonas 4", new List<GuestContact>() { new GuestContact(new Guid("77f64d47-6807-4ad4-8217-b7cf8ab0c9bc"), new Guid("98b6ffbc-c624-4816-a34e-fa7fbca546f9"), GuestContactTypeEnum.Email, "Teste 2") }.AsReadOnly()), "   " }
    };
    
    [Theory]
    [MemberData(nameof(Should_throw_ValidationException_When_Value_is_Empty_Data))]
    public async Task Should_throw_ValidationException_When_Value_is_Empty(Guest guest, string name)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdateGuestContactCmdHandler>();

        dbContext.Add(guest);
        await dbContext.SaveChangesAsync();

        var cmd = new UpdateGuestContactCmd(guest.Contacts.First().Id, guest.Id, GuestContactTypeEnum.Undefined, name);
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    public static List<object[]> Should_Update_a_Set_of_GuestContacts_Data = new() {
        new object[] { new Guest(new Guid("faafd779-e7fd-46ff-8ae2-77afcf9299c0"), "Jonas 3", new List<GuestContact>() { new GuestContact(new Guid("5a6ba43c-033d-4d47-82fc-69e52f0b8433"), new Guid("faafd779-e7fd-46ff-8ae2-77afcf9299c0"), GuestContactTypeEnum.Email, "Teste 1") }.AsReadOnly()), "Teste B1" },
        new object[] { new Guest(new Guid("7e907202-98f1-4b5f-8d55-fa6807296ffc"), "Jonas 4", new List<GuestContact>() { new GuestContact(new Guid("9ae495c7-0063-4356-a1cb-56ac1cf9d089"), new Guid("7e907202-98f1-4b5f-8d55-fa6807296ffc"), GuestContactTypeEnum.Email, "Teste 2") }.AsReadOnly()), "Teste B2" }
    };

    [Theory]
    [MemberData(nameof(Should_Update_a_Set_of_GuestContacts_Data))]
    public async Task Should_Update_a_Set_of_GuestContacts(Guest guest, string value)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<UpdateGuestContactCmdHandler>();
        
        dbContext.Add(guest);
        await dbContext.SaveChangesAsync();

        var cmd = new UpdateGuestContactCmd(guest.Contacts.First().Id, guest.Id, GuestContactTypeEnum.Phone, value);
        await Should.NotThrowAsync(() => handler.Handle(cmd, CancellationToken.None));
        dbContext.GuestContacts.Any(q => q.Value.Equals(value) && q.Type == GuestContactTypeEnum.Phone).ShouldBe(true);
    }
}
