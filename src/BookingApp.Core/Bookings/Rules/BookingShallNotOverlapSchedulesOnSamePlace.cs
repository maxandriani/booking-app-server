using BookingApp.Core.Bookings.Events;
using BookingApp.Core.Bookings.Exceptions;
using BookingApp.Core.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Bookings.Rules;

public class BookingShallNotOverlapSchedulesOnSamePlace :
    INotificationHandler<ValidateConfirmBookingCmd>
{
    private readonly BookingDbContext _dbContext;

    public BookingShallNotOverlapSchedulesOnSamePlace(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Handle(ValidateConfirmBookingCmd notification, CancellationToken cancellationToken)
    {
        var booking = await _dbContext.Bookings.FirstAsync(q => q.Id == notification.BookingId);
        var overlap = await _dbContext.Bookings
            .Include(q => q.Place)
            .WhereOverlapBooking(booking.CheckIn, booking.CheckOut, booking.PlaceId)
            .Where(q => q.Id != notification.BookingId)
            .FirstOrDefaultAsync(cancellationToken);
        if (overlap != null) throw new BookingOverlapException(overlap.CheckIn, overlap.CheckOut, overlap.Place?.Name ?? overlap.PlaceId.ToString());
    }
}