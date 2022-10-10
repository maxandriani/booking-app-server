using BookingApp.Core.Bookings.Events;
using BookingApp.Core.Bookings.Exceptions;
using BookingApp.Core.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Bookings.Rules;

public class BookingShallNotChangePlaceWhenReadOnlyState : INotificationHandler<ValidateUpdateBookingCmd>
{
    private readonly BookingDbContext _dbContext;

    public BookingShallNotChangePlaceWhenReadOnlyState(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(ValidateUpdateBookingCmd notification, CancellationToken cancellationToken)
    {
        var placeIntegrityViolated = await _dbContext.Bookings.AnyAsync(q =>
            q.Id == notification.Booking.Id
            && (q.Status == Models.BookingStatusEnum.Confirmed || q.Status == Models.BookingStatusEnum.Cancelled)
            && q.PlaceId != notification.Booking.PlaceId);
        if (placeIntegrityViolated) throw new BookingReadOnlyStateException(notification.Booking.Id);            
    }
}