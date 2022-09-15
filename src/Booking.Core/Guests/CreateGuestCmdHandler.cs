using Booking.Core.Commons.Handlers;
using Booking.Core.Data;
using Booking.Core.GuestContacts.Models;
using Booking.Core.Guests.Commands;
using Booking.Core.Guests.Events;
using Booking.Core.Guests.Models;
using Booking.Core.Guests.ViewModels;
using FluentValidation;
using MediatR;

namespace Booking.Core.Guests;

public class CreateGuestCmdHandler :
    CreateCmdHandlerBase<BookingDbContext, Guest, CreateGuestWithContactsCmd, GuestWithContactsResponse, CheckingCreateGuestCmdRules>
{
    public CreateGuestCmdHandler(
        BookingDbContext dbContext,
        IValidator<CreateGuestWithContactsCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    {}

    protected override Guest MapToEntity(CreateGuestWithContactsCmd request)
    {
        var (name, contacts) = request;
        var entity = new Guest()
        {
            Name = name
        };

        if (contacts != null)
            entity.Contacts = contacts
                .Select(x =>
                {
                    var (type, value) = x;
                    return new GuestContact() { Type = type, Value = value };
                })
                .ToList();

        return entity;
    }

    protected override GuestWithContactsResponse MapToResponse(Guest entity)
        => new GuestWithContactsResponse(entity);

    protected override CheckingCreateGuestCmdRules MapToEvent(Guest entity)
        => new CheckingCreateGuestCmdRules(entity);
}
