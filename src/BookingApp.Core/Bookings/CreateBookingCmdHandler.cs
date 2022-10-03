using BookingApp.Core.Bookings.Commands;
using BookingApp.Core.Bookings.Events;
using BookingApp.Core.Bookings.ViewModels;
using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using FluentValidation;
using MediatR;

namespace BookingApp.Core.Bookings;

public class CreateBookingCmdHandler : CreateCmdHandlerBase<
    BookingDbContext,
    BookingApp.Core.Bookings.Models.Booking,
    CreateBookingCmd,
    BookingResponse,
    ValidateCreateBookingCmd>
{
    public CreateBookingCmdHandler(
        BookingDbContext dbContext,
        IValidator<CreateBookingCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    {
    }

    protected override Models.Booking MapToEntity(CreateBookingCmd request)
        => new Models.Booking()
        {
            CheckIn = request.CheckIn,
            CheckOut = request.CheckOut,
            Description = request.Description,
            PlaceId = request.PlaceId
        };

    protected override ValidateCreateBookingCmd MapToEvent(Models.Booking entity)
        => new ValidateCreateBookingCmd(entity);

    protected override BookingResponse MapToResponse(Models.Booking entity)
        => new BookingResponse(entity);
}