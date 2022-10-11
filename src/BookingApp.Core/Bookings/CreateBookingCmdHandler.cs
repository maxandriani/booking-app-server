using BookingApp.Core.Bookings.Commands;
using BookingApp.Core.Bookings.Events;
using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Bookings.ViewModels;
using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Bookings;

public class CreateBookingCmdHandler : IRequestHandler<CreateBookingCmd, BookingResponse>
{
    public readonly BookingDbContext _dbContext;
    public readonly IValidator<CreateBookingCmd> _requestValidator;
    public readonly IMediator _mediator;

    public CreateBookingCmdHandler(
        BookingDbContext dbContext,
        IValidator<CreateBookingCmd> requestValidator,
        IMediator mediator)
    {
        _dbContext = dbContext;
        _requestValidator = requestValidator;
        _mediator = mediator;
    }

    public async Task<BookingResponse> Handle(CreateBookingCmd request, CancellationToken cancellationToken)
    {
        await _requestValidator.ValidateAndThrowAsync(request);
        var booking = MapToEntity(request);
        await _mediator.Publish(new ValidateCreateBookingCmd(booking));
        _dbContext.Add(booking);
        await _dbContext.SaveChangesAsync();
        booking.Place = await _dbContext.Places.FirstAsync(q => q.Id == booking.PlaceId);
        return new BookingResponse(booking);
    }

    private Booking MapToEntity(CreateBookingCmd request)
        => new Booking()
        {
            CheckIn = request.CheckIn,
            CheckOut = request.CheckOut,
            Description = request.Description,
            PlaceId = request.PlaceId
        };

}