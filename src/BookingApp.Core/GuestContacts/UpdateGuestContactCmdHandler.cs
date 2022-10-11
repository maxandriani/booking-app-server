using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using BookingApp.Core.GuestContacts.Commands;
using BookingApp.Core.GuestContacts.Events;
using BookingApp.Core.GuestContacts.Models;
using BookingApp.Core.GuestContacts.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.GuestContacts;

public class UpdateGuestContactCmdHandler :
    UpdateCmdHandlerBase<BookingDbContext, GuestContact, UpdateGuestContactCmd, GuestContactResponse, ValidateUpdateGuestContactCmdRules>
{
    public UpdateGuestContactCmdHandler(
        BookingDbContext dbContext,
        IValidator<UpdateGuestContactCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    {
    }

    protected override Task<GuestContact?> GetByKeyAsync(UpdateGuestContactCmd request)
        => _dbContext.GuestContacts.FirstOrDefaultAsync(q => q.Id == request.Id && q.GuestId == request.GuestId);

    protected override ValidateUpdateGuestContactCmdRules MapToEvent(GuestContact entity)
        => new ValidateUpdateGuestContactCmdRules(entity);

    protected override GuestContactResponse MapToResponse(GuestContact entity)
        => new GuestContactResponse(entity);

    protected override void UpdateEntity(UpdateGuestContactCmd request, GuestContact entity)
    {
        var (_, _, type, value) = request;
        entity.Type = type;
        entity.Value = value;
    }
}
