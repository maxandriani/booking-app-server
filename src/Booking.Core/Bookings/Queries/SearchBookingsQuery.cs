using Booking.Core.Bookings.Models;
using Booking.Core.Bookings.ViewModels;
using Booking.Core.Commons.Queries;
using Booking.Core.Commons.ViewModels;
using MediatR;

namespace Booking.Core.Bookings.Queries;

public record SearchBookingsQuery(
    Guid? ByPlace,
    DateTime? CheckIn,
    DateTime? CheckOut,
    BookingStatusEnum? Status,
    string? Search,
    int? Take,
    int? Skip,
    string? SortBy
) : IRequest<CollectionResponse<SearchBookingResponse>>,
    ISortableQuery,
    IPageableQuery;
