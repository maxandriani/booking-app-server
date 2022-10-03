using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using BookingApp.Core.Places.Commands;
using BookingApp.Core.Places.Events;
using BookingApp.Core.Places.Models;
using BookingApp.Core.Places.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Places;

public class UpdatePlaceCmdHandler :
    UpdateCmdHandlerBase<BookingDbContext, Place, UpdatePlaceCmd, PlaceResponse, ValidateUpdatePlaceCmdRules>
{
    public UpdatePlaceCmdHandler(
        BookingDbContext dbContext,
        IValidator<UpdatePlaceCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    {
    }

    protected override Task<Place?> GetByKeyAsync(UpdatePlaceCmd request)
        => _dbContext.Places.FirstOrDefaultAsync(q => q.Id == request.Id);

    protected override ValidateUpdatePlaceCmdRules MapToEvent(Place entity)
        => new ValidateUpdatePlaceCmdRules(entity);

    protected override PlaceResponse MapToResponse(Place entity)
        => new PlaceResponse(entity);

    protected override void UpdateEntity(UpdatePlaceCmd request, Place entity)
    {
        entity.Name = request.Name;
        entity.Address = request.Address;
    }
}