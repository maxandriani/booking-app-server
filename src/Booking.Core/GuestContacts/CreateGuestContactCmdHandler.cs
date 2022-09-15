using Booking.Core.Commons.Handlers;
using Booking.Core.Data;
using Booking.Core.GuestContacts.Commands;
using Booking.Core.GuestContacts.Events;
using Booking.Core.GuestContacts.Models;
using Booking.Core.GuestContacts.ViewModels;
using FluentValidation;
using MediatR;

namespace Booking.Core.GuestContacts;

public class CreateGuestContactCmdHandler :
    CreateCmdHandlerBase<
        BookingDbContext,
        GuestContact,
        CreateGuestContactCmd,
        GuestContactResponse,
        CheckingCreateGuestContactCmdRules>
{
    public CreateGuestContactCmdHandler(
        BookingDbContext dbContext,
        IValidator<CreateGuestContactCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    {
    }

    protected override GuestContact MapToEntity(CreateGuestContactCmd request)
    {
        var (guestId, type, value) = request;
        return new GuestContact()
        {
            GuestId = guestId,
            Type = type,
            Value = value
        };
    }

    protected override CheckingCreateGuestContactCmdRules MapToEvent(GuestContact entity)
        => new CheckingCreateGuestContactCmdRules(entity);

    protected override GuestContactResponse MapToResponse(GuestContact entity)
        => new GuestContactResponse(entity);
}