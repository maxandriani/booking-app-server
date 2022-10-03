using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using BookingApp.Core.GuestContacts.Commands;
using BookingApp.Core.GuestContacts.Events;
using BookingApp.Core.GuestContacts.Models;
using BookingApp.Core.GuestContacts.ViewModels;
using FluentValidation;
using MediatR;

namespace BookingApp.Core.GuestContacts;

public class CreateGuestContactCmdHandler :
    CreateCmdHandlerBase<
        BookingDbContext,
        GuestContact,
        CreateGuestContactCmd,
        GuestContactResponse,
        ValidateCreateGuestContactCmdRules>
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

    protected override ValidateCreateGuestContactCmdRules MapToEvent(GuestContact entity)
        => new ValidateCreateGuestContactCmdRules(entity);

    protected override GuestContactResponse MapToResponse(GuestContact entity)
        => new GuestContactResponse(entity);
}