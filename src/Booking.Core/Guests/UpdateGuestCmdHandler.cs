using Booking.Core.Commons.Exceptions;
using Booking.Core.Commons.Handlers;
using Booking.Core.Commons.Validators;
using Booking.Core.Data;
using Booking.Core.Guests.Commands;
using Booking.Core.Guests.Events;
using Booking.Core.Guests.Models;
using Booking.Core.Guests.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Core.Guests;

public class UpdateGuestCmdHandler :
    UpdateCmdHandlerBase<BookingDbContext, Guest, UpdateGuestCmd, GuestWithContactsResponse, CheckingUpdateGuestCmdRules>
{
    public UpdateGuestCmdHandler(
        BookingDbContext dbContext,
        IValidator<UpdateGuestCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    {
    }

    protected override Task<Guest?> GetByKeyAsync(UpdateGuestCmd request)
        => _dbContext.Guests
            .Include(q => q.Contacts)
            .FirstOrDefaultAsync(q => q.Id == request.Id);

    protected override CheckingUpdateGuestCmdRules MapToEvent(Guest entity)
        => new CheckingUpdateGuestCmdRules(entity);

    protected override GuestWithContactsResponse MapToResponse(Guest entity)
        => new GuestWithContactsResponse(entity);

    protected override void UpdateEntity(UpdateGuestCmd request, Guest entity)
    {
        var (_, name) = request;
        entity.Name = name;
    }
}