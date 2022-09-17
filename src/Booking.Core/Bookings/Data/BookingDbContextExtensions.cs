using Booking.Core.Bookings.Models;

namespace Booking.Core.Data;

public static class BookingDbContextExtensions
{
    public static IQueryable<Bookings.Models.Booking> WhereOverlapBooking(this IQueryable<Bookings.Models.Booking> query, DateTime checkIn, DateTime checkOut)
        => query.Where(
            q => (q.CheckIn >= checkIn && q.CheckOut <= checkIn || q.CheckIn >= checkOut && q.CheckOut <= checkOut)
            && q.Status == BookingStatusEnum.Confirmed);

    public static IQueryable<Bookings.Models.Booking> WhereOverlapBooking(this IQueryable<Bookings.Models.Booking> query, DateTime checkIn, DateTime checkOut, Guid placeId)
        => query
            .WhereOverlapBooking(checkIn, checkOut)
            .Where(q => q.PlaceId == placeId);
}