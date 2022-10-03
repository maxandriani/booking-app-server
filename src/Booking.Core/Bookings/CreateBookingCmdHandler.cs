using Booking.Core.Bookings.Commands;
using Booking.Core.Bookings.Events;
using Booking.Core.Bookings.ViewModels;
using Booking.Core.Commons.Handlers;
using Booking.Core.Data;
using FluentValidation;
using MediatR;

namespace Booking.Core.Bookings;

public class CreateBookingCmdHandler : CreateCmdHandlerBase<
    BookingDbContext,
    Booking.Core.Bookings.Models.Booking,
    CreateBookingCmd,
    BookingResponse,
    CheckingCreateBookingCmd>
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

    protected override CheckingCreateBookingCmd MapToEvent(Models.Booking entity)
        => new CheckingCreateBookingCmd(entity);

    protected override BookingResponse MapToResponse(Models.Booking entity)
        => new BookingResponse(entity);
}