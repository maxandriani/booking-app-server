using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using BookingApp.Core.Guests.Commands;
using BookingApp.Core.Guests.Events;
using BookingApp.Core.Guests.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Guests;

public class DeleteGuestCmdHandler :
    DeleteCmdHandlerBase<BookingDbContext, Guest, DeleteGuestCmd, ValidateDeleteGuestCmdRules>
{
    public DeleteGuestCmdHandler(
        BookingDbContext dbContext,
        IValidator<DeleteGuestCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    {
    }

    protected override Task<Guest?> GetByKeyAsync(DeleteGuestCmd request)
        => _dbContext.Guests.FirstOrDefaultAsync(q => q.Id == request.Id);

    protected override ValidateDeleteGuestCmdRules MapToEvent(Guest entity)
        => new ValidateDeleteGuestCmdRules(entity);
}