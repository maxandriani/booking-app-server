using BookingApp.Core.Bookings.Commands;
using BookingApp.Core.Bookings.Events;
using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Bookings;

public class DeleteBookingCmdHandler : DeleteCmdHandlerBase<
    BookingDbContext,
    Booking,
    DeleteBookingCmd,
    ValidateDeleteBookingCmd>
{
    public DeleteBookingCmdHandler(
        BookingDbContext dbContext,
        IValidator<DeleteBookingCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    {
    }

    protected override Task<Booking?> GetByKeyAsync(DeleteBookingCmd request)
        => _dbContext.Bookings.FirstOrDefaultAsync(q => q.Id == request.Id);

    protected override ValidateDeleteBookingCmd MapToEvent(Booking entity)
        => new ValidateDeleteBookingCmd(entity);
}