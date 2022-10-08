using BookingApp.Core.Bookings.Events;
using BookingApp.Core.Bookings.Exceptions;
using BookingApp.Core.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Bookings;

public class BookingShallNotBeDeletedInConfirmedOrCancelledState : INotificationHandler<ValidateDeleteBookingCmd>
{
    private readonly BookingDbContext _dbContext;

    public BookingShallNotBeDeletedInConfirmedOrCancelledState(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(ValidateDeleteBookingCmd notification, CancellationToken cancellationToken)
    {
        var isReadOnlyState = await _dbContext.Bookings.AnyAsync(q => q.Id == notification.Booking.Id && (
            q.Status == Models.BookingStatusEnum.Confirmed || q.Status == Models.BookingStatusEnum.Cancelled));
        if (isReadOnlyState) throw new BookingReadOnlyStateException(notification.Booking.Id);
    }
}