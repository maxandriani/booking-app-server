using Booking.Core.Commons.Handlers;
using Booking.Core.Data;
using Booking.Core.Places.Commands;
using Booking.Core.Places.Events;
using Booking.Core.Places.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Core.Places;

public class DeletePlaceCmdHandler :
    DeleteCmdHandlerBase<BookingDbContext, Place, DeletePlaceCmd, CheckingDeletePlaceCmdRules>
{
    public DeletePlaceCmdHandler(
        BookingDbContext dbContext,
        IValidator<DeletePlaceCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    {
    }

    protected override Task<Place?> GetByKeyAsync(DeletePlaceCmd request)
        => _dbContext.Places.FirstOrDefaultAsync(q => q.Id == request.Id);

    protected override CheckingDeletePlaceCmdRules MapToEvent(Place entity)
        => new CheckingDeletePlaceCmdRules(entity);
}