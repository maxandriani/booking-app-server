using Booking.Core.Data;
using Booking.Core.GuestContacts.Models;
using Booking.Core.Guests;
using Booking.Core.Guests.Commands;
using Booking.Core.Tests.Commons;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Shouldly;

namespace Booking.Core.Tests.Guests;

public class CreateGuestCmdHandlerTest : TestBase
{

    public static List<object[]> Should_throw_ValidationException_When_Guest_Name_Less_Than_3_Data = new() {
        new object[] { new CreateGuestWithContactsCmd("J" ) },
        new object[] { new CreateGuestWithContactsCmd("Jo" ) },
        new object[] { new CreateGuestWithContactsCmd("" ) },
        new object[] { new CreateGuestWithContactsCmd("   " ) },
        new object[] { new CreateGuestWithContactsCmd("   1" ) },
        new object[] { new CreateGuestWithContactsCmd("1   " ) }
    };
    
    [Theory]
    [MemberData(nameof(Should_throw_ValidationException_When_Guest_Name_Less_Than_3_Data))]
    public async Task Should_throw_ValidationException_When_Guest_Name_Less_Than_3(CreateGuestWithContactsCmd cmd)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<CreateGuestCmdHandler>();

        await Should.ThrowAsync<ValidationException>(() => handler.Handle(cmd, CancellationToken.None));
    }

    public static List<object[]> Should_Add_a_Set_of_Guests_Data = new() {
        new object[] { new CreateGuestWithContactsCmd("Jonas 1") },
        new object[] { new CreateGuestWithContactsCmd("Jonas 1") },
        new object[] { new CreateGuestWithContactsCmd("Jonas 2") }
    };

    [Theory]
    [MemberData(nameof(Should_Add_a_Set_of_Guests_Data))]
    public async Task Should_Add_a_Set_of_Guests(CreateGuestWithContactsCmd cmd)
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<CreateGuestCmdHandler>();

        await Should.NotThrowAsync(() => handler.Handle(cmd, CancellationToken.None));
        dbContext.Guests.Any(q => q.Name.Equals(cmd.Name)).ShouldBe(true);
    }

    [Fact]
    public async Task Should_Add_a_Guest_With_Contacts()
    {
        var dbContext = _injector.GetRequiredService<BookingDbContext>();
        var handler = _injector.GetRequiredService<CreateGuestCmdHandler>();
        var cmd = new CreateGuestWithContactsCmd("Jonas Teste", new List<CreateGuestContact>
        {
            new CreateGuestContact(GuestContactTypeEnum.Email, "Contact 1"),
            new CreateGuestContact(GuestContactTypeEnum.Undefined, "Contact 2"),
            new CreateGuestContact(GuestContactTypeEnum.Phone, "Contact 3"),
        });

        await Should.NotThrowAsync(() => handler.Handle(cmd, CancellationToken.None));
        dbContext.Guests.Any(q => q.Name.Equals(cmd.Name)).ShouldBe(true);
        dbContext.GuestContacts.Include(q => q.Guest).Any(q => q.Guest!.Name.Equals(cmd.Name) && q.Type == GuestContactTypeEnum.Undefined && q.Value.Equals("Contact 2"));
    }
}
