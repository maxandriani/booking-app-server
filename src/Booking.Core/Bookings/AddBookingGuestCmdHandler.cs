using Booking.Core.Bookings.Commands;
using Booking.Core.Bookings.Events;
using Booking.Core.Bookings.Models;
using Booking.Core.Commons.Exceptions;
using Booking.Core.Data;
using Booking.Core.Guests.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Core.Bookings;

public class AddBookingGuestCmdHandler : IRequestHandler<AddBookingGuestCmd>
{
    private readonly BookingDbContext _dbContext;
    private readonly IValidator<AddBookingGuestCmd> _requestValidator;
    private readonly IMediator _mediator;

    public AddBookingGuestCmdHandler(
        BookingDbContext dbContext,
        IValidator<AddBookingGuestCmd> requestValidator,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _requestValidator = requestValidator;
        _mediator = mediator;
    }

    public async Task<Unit> Handle(AddBookingGuestCmd request, CancellationToken cancellationToken)
    {
        await _requestValidator.ValidateAndThrowAsync(request, cancellationToken);
        var hasBooking = await _dbContext.Bookings.AnyAsync(q => q.Id == request.BookingId, cancellationToken);
        if (!hasBooking) throw new ResourceNotFoundException(nameof(Booking.Core.Bookings.Models.Booking));
        
        var hasGuest = await _dbContext.Guests.AnyAsync(q => q.Id == request.GuestId, cancellationToken);
        if (!hasGuest) throw new ResourceNotFoundException(nameof(Guest));

        var primary = !(await _dbContext.BookingGuests.AnyAsync(q => q.BookingId == request.BookingId));
        var entity = new BookingGuest(request.BookingId, request.GuestId, primary);

        await _mediator.Publish(new CheckingAddBookingGuestCmd(request.BookingId, request.GuestId));

        _dbContext.BookingGuests.Add(entity);
        await _dbContext.SaveChangesAsync();

        return Unit.Value;
    }
}
