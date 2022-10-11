using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using BookingApp.Core.Places.Commands;
using BookingApp.Core.Places.Events;
using BookingApp.Core.Places.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Places;

public class DeletePlaceCmdHandler :
    DeleteCmdHandlerBase<BookingDbContext, Place, DeletePlaceCmd, ValidateDeletePlaceCmdRules>
{
    public DeletePlaceCmdHandler(
        BookingDbContext dbContext,
        IValidator<DeletePlaceCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    {
    }

    protected override Task<Place?> GetByKeyAsync(DeletePlaceCmd request)
        => _dbContext.Places.FirstOrDefaultAsync(q => q.Id == request.Id);

    protected override ValidateDeletePlaceCmdRules MapToEvent(Place entity)
        => new ValidateDeletePlaceCmdRules(entity);
}