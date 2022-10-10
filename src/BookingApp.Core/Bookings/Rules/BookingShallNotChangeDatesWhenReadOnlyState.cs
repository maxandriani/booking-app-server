using BookingApp.Core.Bookings.Events;
using BookingApp.Core.Bookings.Exceptions;
using BookingApp.Core.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Bookings.Rules;

public class BookingShallNotChangeDatesWhenReadOnlyState : INotificationHandler<ValidateUpdateBookingCmd>
{
    private readonly BookingDbContext _dbContext;

    public BookingShallNotChangeDatesWhenReadOnlyState(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(ValidateUpdateBookingCmd notification, CancellationToken cancellationToken)
    {
        var datesIntegrityViolated = await _dbContext.Bookings.AnyAsync(q =>
            q.Id == notification.Booking.Id
            && (q.Status == Models.BookingStatusEnum.Confirmed || q.Status == Models.BookingStatusEnum.Cancelled)
            && (q.CheckIn != notification.Booking.CheckIn || q.CheckOut != notification.Booking.CheckOut));
        if (datesIntegrityViolated) throw new BookingReadOnlyStateException(notification.Booking.Id);            
    }
}