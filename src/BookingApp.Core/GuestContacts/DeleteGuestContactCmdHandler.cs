using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using BookingApp.Core.GuestContacts.Commands;
using BookingApp.Core.GuestContacts.Events;
using BookingApp.Core.GuestContacts.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.GuestContacts;

public class DeleteGuestContactCmdHandler :
    DeleteCmdHandlerBase<BookingDbContext, GuestContact, DeleteGuestContactCmd, ValidateDeleteGuestContactCmdRules>
{
    public DeleteGuestContactCmdHandler(
        BookingDbContext dbContext,
        IValidator<DeleteGuestContactCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    {
    }

    protected override Task<GuestContact?> GetByKeyAsync(DeleteGuestContactCmd request)
        => _dbContext.GuestContacts.FirstOrDefaultAsync(q => q.Id == request.Id && q.GuestId == request.GuestId);

    protected override ValidateDeleteGuestContactCmdRules MapToEvent(GuestContact entity)
        => new ValidateDeleteGuestContactCmdRules(entity);
}