using Booking.Core.Commons.Handlers;
using Booking.Core.Data;
using Booking.Core.GuestContacts.Commands;
using Booking.Core.GuestContacts.Events;
using Booking.Core.GuestContacts.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Core.GuestContacts;

public class DeleteGuestContactCmdHandler :
    DeleteCmdHandlerBase<BookingDbContext, GuestContact, DeleteGuestContactCmd, CheckingDeleteGuestContactCmdRules>
{
    public DeleteGuestContactCmdHandler(
        BookingDbContext dbContext,
        IValidator<DeleteGuestContactCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    {
    }

    protected override Task<GuestContact?> GetByKeyAsync(DeleteGuestContactCmd request)
        => _dbContext.GuestContacts.FirstOrDefaultAsync(q => q.Id == request.Id && q.GuestId == request.GuestId);

    protected override CheckingDeleteGuestContactCmdRules MapToEvent(GuestContact entity)
        => new CheckingDeleteGuestContactCmdRules(entity);
}