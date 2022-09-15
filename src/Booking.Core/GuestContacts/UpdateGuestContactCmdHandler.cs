using Booking.Core.Commons.Handlers;
using Booking.Core.Data;
using Booking.Core.GuestContacts.Commands;
using Booking.Core.GuestContacts.Events;
using Booking.Core.GuestContacts.Models;
using Booking.Core.GuestContacts.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Core.GuestContacts;

public class UpdateGuestContactCmdHandler :
    UpdateCmdHandlerBase<BookingDbContext, GuestContact, UpdateGuestContactCmd, GuestContactResponse, CheckingUpdateGuestContactCmdRules>
{
    public UpdateGuestContactCmdHandler(
        BookingDbContext dbContext,
        IValidator<UpdateGuestContactCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    {
    }

    protected override Task<GuestContact?> GetByKeyAsync(UpdateGuestContactCmd request)
        => _dbContext.GuestContacts.FirstOrDefaultAsync(q => q.Id == request.Id && q.GuestId == request.GuestId);

    protected override CheckingUpdateGuestContactCmdRules MapToEvent(GuestContact entity)
        => new CheckingUpdateGuestContactCmdRules(entity);

    protected override GuestContactResponse MapToResponse(GuestContact entity)
        => new GuestContactResponse(entity);

    protected override void UpdateEntity(UpdateGuestContactCmd request, GuestContact entity)
    {
        var (_, _, type, value) = request;
        entity.Type = type;
        entity.Value = value;
    }
}
