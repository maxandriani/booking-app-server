using BookingApp.Core.Bookings.Commands;
using BookingApp.Core.Bookings.Events;
using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Bookings;

public class UnConfirmBookingCmdHandler : IRequestHandler<UnConfirmBookingCmd>
{
    private readonly BookingDbContext _dbContext;
    private readonly IValidator<UnConfirmBookingCmd> _requestValidator;
    private readonly IMediator _mediator;

    public UnConfirmBookingCmdHandler(
        BookingDbContext dbContext,
        IValidator<UnConfirmBookingCmd> requestValidator,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _requestValidator = requestValidator;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(UnConfirmBookingCmd request, CancellationToken cancellationToken)
    {
        await _requestValidator.ValidateAndThrowAsync(request);
        var entity = await _dbContext.Bookings.FirstOrDefaultAsync(q => q.Id == request.BookingId);
        if (entity == null) throw new ResourceNotFoundException(nameof(Booking));
        await _mediator.Publish(new ValidateUnConfirmBookingCmd(request.BookingId));
        entity.Status = BookingStatusEnum.Pending;
        _dbContext.Update(entity);
        await _dbContext.SaveChangesAsync();

        return Unit.Value;
    }
}
