using Booking.Core.Commons.Handlers;
using Booking.Core.Data;
using Booking.Core.Places.Commands;
using Booking.Core.Places.Events;
using Booking.Core.Places.Models;
using Booking.Core.Places.ViewModels;
using FluentValidation;
using MediatR;

namespace Booking.Core.Places;

public class CreatePlaceCmdHandler :
    CreateCmdHandlerBase<BookingDbContext, Place, CreatePlaceCmd, PlaceResponse, CheckingCreatePlaceCmdRules>
{
    public CreatePlaceCmdHandler(
        BookingDbContext dbContext,
        IValidator<CreatePlaceCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    {
    }

    protected override Place MapToEntity(CreatePlaceCmd request)
        => new Place() { Name = request.Name, Address = request.Address };

    protected override CheckingCreatePlaceCmdRules MapToEvent(Place entity)
        => new CheckingCreatePlaceCmdRules(entity);

    protected override PlaceResponse MapToResponse(Place entity)
        => new PlaceResponse(entity);
}