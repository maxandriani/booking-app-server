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

namespace Booking.Core.Tests.GuestContacts;

public class CreateGuestContactCmdHandlerTest : BaseTest
{
    public static List<object[]> Should_throw_ValidationException_When_GuestContact_Name_is_Empty_Data = new() {
        new object[] { new Guest(new Guid("a7475ef3-2483-4cff-ac30-984c90188cc9"), "abc"), new CreateGuestContactCmd(new Guid("a7475ef3-2483-4cff-ac30-984c90188cc9"), GuestContactTypeEnum.Email, "") },
        new object[] { new Guest(new Guid("fa445ae3-dce5-4e75-98ef-f6cf844c1dab"), "abc"), new CreateGuestContactCmd(new Guid("fa445ae3-dce5-4e75-98ef-f6cf844c1dab"), GuestContactTypeEnum.Phone, " ") },
        new object[] { new Guest(new Guid("1c5d60a3-79eb-4480-9cdf-89d94802a755"), "abc"), new CreateGuestContactCmd(new Guid("1c5d60a3-79eb-4480-9cdf-89d94802a755"), GuestContactTypeEnum.Undefined, "  ") },
        new object[] { new Guest(new Guid("c78ec23e-a96d-4b76-81c0-eeaf81417df1"), "abc"), new CreateGuestContactCmd(new Guid("c78ec23e-a96d-4b76-81c0-eeaf81417df1"), GuestContactTypeEnum.Email, "   ") },
        new object[] { new Guest(new Guid("e8000f3f-f31d-4cb9-9845-0fc63e3729c7"), "abc"), new CreateGuestContactCmd(new Guid("e8000f3f-f31d-4cb9-9845-0fc63e3729c7"), GuestContactTypeEnum.Phone, "    ") },
        new object[] { new Guest(new Guid("dbe1f105-8d3e-4563-b02e-602f33d43c30"), "abc"), new CreateGuestContactCmd(new Guid("dbe1f105-8d3e-4563-b02e-602f33d43c30"), GuestContactTypeEnum.Undefined, "     \n") }
    };
    
    [Theory]
    [MemberData(nameof(Should_throw_ValidationException_When_GuestContact_Name_is_Empty_Data))]
    public async Task Should_throw_ValidationException_When_GuestContact_Name_is_Empty(Guest guest, CreateGuestContactCmd cmd)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<CreateGuestContactCmdHandler>();

        dbContext.Guests.Add(guest);
        await dbContext.AddRangeAsync();

        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    public static List<object[]> Should_Add_a_Set_of_GuestContacts_Data = new() {
        new object[] { new Guest(new Guid("ef089957-17bf-49c6-97ee-d69727eedaf7"), "Max 1", null), new CreateGuestContactCmd(new Guid("ef089957-17bf-49c6-97ee-d69727eedaf7"), GuestContactTypeEnum.Email, "max.andriani@gmail.com") },
        new object[] { new Guest(new Guid("21832b07-0b45-4bbc-a407-790ebb1c734e"), "Max 2", null), new CreateGuestContactCmd(new Guid("21832b07-0b45-4bbc-a407-790ebb1c734e"), GuestContactTypeEnum.Phone, "+55 47 996069134 ") },
        new object[] { new Guest(new Guid("bdf7df2f-ae31-4564-ae7b-bfe351b8928d"), "Max 3", null), new CreateGuestContactCmd(new Guid("bdf7df2f-ae31-4564-ae7b-bfe351b8928d"), GuestContactTypeEnum.Undefined, " https://linkedin.com/in/maxandriani") }
    };

    [Theory]
    [MemberData(nameof(Should_Add_a_Set_of_GuestContacts_Data))]
    public async Task Should_Add_a_Set_of_GuestContacts(Guest guest, CreateGuestContactCmd cmd)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<CreateGuestContactCmdHandler>();

        dbContext.Guests.Add(guest);
        await dbContext.SaveChangesAsync();

        await Should.NotThrowAsync(() => handler.Handle(cmd, CancellationToken.None));
        dbContext.GuestContacts.Any(q => q.GuestId == guest.Id && q.Type == cmd.Type && q.Value == cmd.Value.Trim()).ShouldBe(true);
    }

    [Fact]
    public async Task Should_not_add_GuestContact_wo_Guest()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<CreateGuestContactCmdHandler>();

        var cmd = new CreateGuestContactCmd(new Guid("daef358c-c922-4f0f-bd13-3cd7c9a3b3b3"), GuestContactTypeEnum.Email, "Test");
        await Should.ThrowAsync<ResourceNotFoundException>(() => handler.Handle(cmd, CancellationToken.None));
        dbContext.GuestContacts.Any(q => q.GuestId == cmd.GuestId && q.Type == cmd.Type && q.Value.Equals(cmd.Value)).ShouldBe(false);
    }

    [Fact]
    public async Task Should_not_add_GuestContact_w_Empty_Guest()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<CreateGuestContactCmdHandler>();

        var cmd = new CreateGuestContactCmd(Guid.Empty, GuestContactTypeEnum.Email, "Test");
        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }
}
