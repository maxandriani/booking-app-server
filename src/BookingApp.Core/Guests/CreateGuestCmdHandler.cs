using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using BookingApp.Core.GuestContacts.Models;
using BookingApp.Core.Guests.Commands;
using BookingApp.Core.Guests.Events;
using BookingApp.Core.Guests.Models;
using BookingApp.Core.Guests.ViewModels;
using FluentValidation;
using MediatR;

namespace BookingApp.Core.Guests;

public class CreateGuestCmdHandler :
    CreateCmdHandlerBase<BookingDbContext, Guest, CreateGuestWithContactsCmd, GuestWithContactsResponse, ValidateCreateGuestCmdRules>
{
    public CreateGuestCmdHandler(
        BookingDbContext dbContext,
        IValidator<CreateGuestWithContactsCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    { }

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

    protected override ValidateCreateGuestCmdRules MapToEvent(Guest entity)
        => new ValidateCreateGuestCmdRules(entity);
}
