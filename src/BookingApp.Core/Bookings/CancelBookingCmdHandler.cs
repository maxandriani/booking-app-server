using BookingApp.Core.Bookings.Commands;
using BookingApp.Core.Bookings.Events;
using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Bookings;

public class CancelBookingCmdHandler : IRequestHandler<CancelBookingCmd>
{
    private readonly IMediator _mediator;
    private readonly BookingDbContext _dbContext;
    private readonly IValidator<CancelBookingCmd> _requestValidator;

    public CancelBookingCmdHandler(
        IMediator mediator,
        BookingDbContext dbContext,
        IValidator<CancelBookingCmd> requestValidator)
    {
        _mediator = mediator;
        _dbContext = dbContext;
        _requestValidator = requestValidator;
    }

    public async Task<Unit> Handle(CancelBookingCmd request, CancellationToken cancellationToken)
    {
        await _requestValidator.ValidateAndThrowAsync(request, cancellationToken);
        var booking = await _dbContext.Bookings.FirstOrDefaultAsync(q => q.Id == request.BookingId);
        if (booking == null) throw new ResourceNotFoundException(nameof(Booking));
        await _mediator.Publish(new ValidateCancelBookingCmd(request.BookingId));
        booking.Status = BookingStatusEnum.Cancelled;
        _dbContext.Bookings.Update(booking);
        await _dbContext.SaveChangesAsync();
        return Unit.Value;
    }
}
