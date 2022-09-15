using Booking.Core.Commons.Handlers;
using Booking.Core.Data;
using Booking.Core.Guests.Commands;
using Booking.Core.Guests.Events;
using Booking.Core.Guests.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Core.Guests;

public class DeleteGuestCmdHandler :
    DeleteCmdHandlerBase<BookingDbContext, Guest, DeleteGuestCmd, CheckingDeleteGuestCmdRules>
{
    public DeleteGuestCmdHandler(
        BookingDbContext dbContext,
        IValidator<DeleteGuestCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    {
    }

    protected override Task<Guest?> GetByKeyAsync(DeleteGuestCmd request)
        => _dbContext.Guests.FirstOrDefaultAsync(q => q.Id == request.Id);

    protected override CheckingDeleteGuestCmdRules MapToEvent(Guest entity)
        => new CheckingDeleteGuestCmdRules(entity);
}