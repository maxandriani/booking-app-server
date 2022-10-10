using BookingApp.Core.Bookings.Commands;
using BookingApp.Core.Bookings.Events;
using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Bookings.ViewModels;
using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.Data;
using BookingApp.Core.Places.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Bookings;

public class UpdateBookingCmdHandler : IRequestHandler<UpdateBookingCmd, BookingResponse>
{
    private readonly IMediator _mediator;
    private readonly IValidator<UpdateBookingCmd> _requestValidator;
    private readonly BookingDbContext _dbContext;

    public UpdateBookingCmdHandler(
        IMediator mediator,
        IValidator<UpdateBookingCmd> requestValidator,
        BookingDbContext dbContext)
    {
        _mediator = mediator;
        _requestValidator = requestValidator;
        _dbContext = dbContext;
    }

    public async Task<BookingResponse> Handle(UpdateBookingCmd request, CancellationToken cancellationToken)
    {
        await _requestValidator.ValidateAndThrowAsync(request);
        var booking = await _dbContext
            .Bookings
            .Include(q => q.Guests)
                .ThenInclude(q => q.Guest)
            .Include(q => q.Place)
            .FirstOrDefaultAsync(q => q.Id == request.Id);
        if (booking == null) throw new ResourceNotFoundException(nameof(Booking));
        var (_, placeId, checkIn, checkOut, description) = request;
        booking.CheckIn = checkIn;
        booking.CheckOut = checkOut;
        booking.Description = description;
        if (booking.PlaceId != placeId)
        {
            var place = await _dbContext.Places.FirstOrDefaultAsync(q => q.Id == placeId);
            if (place == null) throw new ResourceNotFoundException(nameof(Place));
            booking.Place = place;
        }
        await _mediator.Publish(new ValidateUpdateBookingCmd(booking));
        _dbContext.Update(booking);
        await _dbContext.SaveChangesAsync();
        return new BookingResponse(booking);
    }
}