using BookingApp.Core.Bookings.Commands;
using BookingApp.Core.Bookings.Events;
using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Bookings;

public class ConfirmBookingCmdHandler : IRequestHandler<ConfirmBookingCmd>
{
    private readonly IMediator _mediator;
    private readonly BookingDbContext _dbContext;
    private readonly IValidator<ConfirmBookingCmd> _requestValidator;

    public ConfirmBookingCmdHandler(
        IMediator mediator,
        BookingDbContext dbContext,
        IValidator<ConfirmBookingCmd> requestValidator)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _requestValidator = requestValidator;
    }

    public async Task<Unit> Handle(ConfirmBookingCmd request, CancellationToken cancellationToken)
    {
        await _requestValidator.ValidateAndThrowAsync(request);
        var booking = await _dbContext.Bookings.FirstOrDefaultAsync(q => q.Id == request.BookingId, cancellationToken);
        if (booking == null) throw new ResourceNotFoundException(nameof(Booking));
        await _mediator.Publish(new ValidateConfirmBookingCmd(request.BookingId));
        booking.Status = BookingStatusEnum.Confirmed;
        _dbContext.Bookings.Update(booking);
        await _dbContext.SaveChangesAsync();
        return Unit.Value;
    }
}
