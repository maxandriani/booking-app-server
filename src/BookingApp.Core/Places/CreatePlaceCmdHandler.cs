using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using BookingApp.Core.Places.Commands;
using BookingApp.Core.Places.Events;
using BookingApp.Core.Places.Models;
using BookingApp.Core.Places.ViewModels;
using FluentValidation;
using MediatR;

namespace BookingApp.Core.Places;

public class CreatePlaceCmdHandler :
    CreateCmdHandlerBase<BookingDbContext, Place, CreatePlaceCmd, PlaceResponse, ValidateCreatePlaceCmdRules>
{
    public CreatePlaceCmdHandler(
        BookingDbContext dbContext,
        IValidator<CreatePlaceCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    {
    }

    protected override Place MapToEntity(CreatePlaceCmd request)
        => new Place() { Name = request.Name, Address = request.Address };

    protected override ValidateCreatePlaceCmdRules MapToEvent(Place entity)
        => new ValidateCreatePlaceCmdRules(entity);

    protected override PlaceResponse MapToResponse(Place entity)
        => new PlaceResponse(entity);
}