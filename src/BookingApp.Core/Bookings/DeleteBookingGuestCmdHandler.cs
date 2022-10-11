using BookingApp.Core.Bookings.Commands;
using BookingApp.Core.Bookings.Events;
using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.Data;
using BookingApp.Core.Guests.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Bookings;

public class DeleteBookingGuestCmdHandler : IRequestHandler<DeleteBookingGuestCmd>
{
    private readonly BookingDbContext _dbContext;
    private readonly IMediator _mediator;
    private readonly IValidator<DeleteBookingGuestCmd> _requestValidator;

    public DeleteBookingGuestCmdHandler(
        BookingDbContext dbContext,
        IMediator mediator,
        IValidator<DeleteBookingGuestCmd> requestValidator)
    {
        _dbContext = dbContext;
        _mediator = mediator;
        _requestValidator = requestValidator;
    }

    public async Task<Unit> Handle(DeleteBookingGuestCmd request, CancellationToken cancellationToken)
    {
        await _requestValidator.ValidateAndThrowAsync(request);
        var bookingGuest = await _dbContext.BookingGuests.FirstOrDefaultAsync(q => q.BookingId == request.BookingId && q.GuestId == request.GuestId);
        
        if (bookingGuest == null)
        {
            if (!await _dbContext.Bookings.AnyAsync(q => q.Id == request.BookingId)) throw new ResourceNotFoundException(nameof(Booking));
            throw new ResourceNotFoundException(nameof(Guest));
        }

        await _mediator.Publish(new ValidateDeleteBookingGuestCmd(request.BookingId, request.GuestId));

        if (bookingGuest.IsPrincipal)
        {
            var principalCandidate = await _dbContext.BookingGuests.FirstOrDefaultAsync(q => q.BookingId == request.BookingId && q.GuestId != request.GuestId && q.IsPrincipal == false);
            if (principalCandidate != null)
            {
                principalCandidate.IsPrincipal = true;
                _dbContext.Update(principalCandidate);
            }
        }

        _dbContext.Remove(bookingGuest);
        await _dbContext.SaveChangesAsync();

        return Unit.Value;
    }
}
