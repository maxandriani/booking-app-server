using BookingApp.Core.Bookings.Events;
using BookingApp.Core.Bookings.Exceptions;
using BookingApp.Core.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Bookings.Rules;

public class BookingShallBeConfirmed :
    INotificationHandler<ValidateCancelBookingCmd>
{
    private readonly BookingDbContext _dbContext;

    public BookingShallBeConfirmed(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(ValidateCancelBookingCmd notification, CancellationToken cancellationToken)
    {
        var isConfirmed = await _dbContext.Bookings.AnyAsync(q => q.Id == notification.BookingId && q.Status == Models.BookingStatusEnum.Confirmed);
        if (!isConfirmed) throw new BookingNotConfirmedException(notification.BookingId);
    }
}