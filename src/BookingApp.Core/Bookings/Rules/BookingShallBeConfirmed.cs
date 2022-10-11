using BookingApp.Core.Bookings.Events;
using BookingApp.Core.Bookings.Exceptions;
using BookingApp.Core.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Bookings.Rules;

public class BookingShallBeConfirmed :
    INotificationHandler<ValidateCancelBookingCmd>,
    INotificationHandler<ValidateUnConfirmBookingCmd>
{
    private readonly BookingDbContext _dbContext;

    public BookingShallBeConfirmed(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private async Task Handle(Guid bookingId, CancellationToken cancellationToken)
    {
        var isConfirmed = await _dbContext.Bookings.AnyAsync(q => q.Id == bookingId && q.Status == Models.BookingStatusEnum.Confirmed);
        if (!isConfirmed) throw new BookingNotConfirmedException(bookingId);
    }

    public Task Handle(ValidateCancelBookingCmd notification, CancellationToken cancellationToken)
        => Handle(notification.BookingId, cancellationToken);

    public Task Handle(ValidateUnConfirmBookingCmd notification, CancellationToken cancellationToken)
        => Handle(notification.BookingId, cancellationToken);
}