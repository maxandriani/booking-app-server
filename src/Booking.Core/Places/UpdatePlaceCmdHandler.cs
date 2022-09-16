using Booking.Core.Commons.Handlers;
using Booking.Core.Data;
using Booking.Core.Places.Commands;
using Booking.Core.Places.Events;
using Booking.Core.Places.Models;
using Booking.Core.Places.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Core.Places;

public class UpdatePlaceCmdHandler :
    UpdateCmdHandlerBase<BookingDbContext, Place, UpdatePlaceCmd, PlaceResponse, CheckingUpdatePlaceCmdRules>
{
    public UpdatePlaceCmdHandler(
        BookingDbContext dbContext,
        IValidator<UpdatePlaceCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    {
    }

    protected override Task<Place?> GetByKeyAsync(UpdatePlaceCmd request)
        => _dbContext.Places.FirstOrDefaultAsync(q => q.Id == request.Id);

    protected override CheckingUpdatePlaceCmdRules MapToEvent(Place entity)
        => new CheckingUpdatePlaceCmdRules(entity);

    protected override PlaceResponse MapToResponse(Place entity)
        => new PlaceResponse(entity);

    protected override void UpdateEntity(UpdatePlaceCmd request, Place entity)
    {
        entity.Name = request.Name;
        entity.Address = request.Address;
    }
}