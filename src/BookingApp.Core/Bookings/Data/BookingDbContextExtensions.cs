using BookingApp.Core.Bookings.Models;

namespace BookingApp.Core.Data;

public static class BookingDbContextExtensions
{
    public static IQueryable<Bookings.Models.Booking> WhereOverlapBooking(this IQueryable<Bookings.Models.Booking> query, DateTime checkIn, DateTime checkOut)
        => query.Where(
            q => (
                    q.CheckIn.Date >= checkIn.Date && q.CheckIn.Date < checkOut.AddDays(1).Date
                    ||
                    q.CheckOut.Date >= checkIn.Date && q.CheckOut.Date < checkOut.AddDays(1).Date
                    ||
                    q.CheckIn.Date <= checkIn.Date && q.CheckOut.Date > checkOut.AddDays(1).Date
                 )
                 &&
                 q.Status == BookingStatusEnum.Confirmed);

    public static IQueryable<Bookings.Models.Booking> WhereOverlapBooking(this IQueryable<Bookings.Models.Booking> query, DateTime checkIn, DateTime checkOut, Guid placeId)
        => query
            .WhereOverlapBooking(checkIn, checkOut)
            .Where(q => q.PlaceId == placeId);
}