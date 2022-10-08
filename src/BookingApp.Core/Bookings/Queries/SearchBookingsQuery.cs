using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Bookings.ViewModels;
using BookingApp.Core.Commons.Queries;
using BookingApp.Core.Commons.ViewModels;
using MediatR;

namespace BookingApp.Core.Bookings.Queries;

// public record SearchBookingsQuery(
//     Guid? ByPlace,
//     DateTime? CheckIn,
//     DateTime? CheckOut,
//     BookingStatusEnum? Status,
//     string? Search,
//     int? Take,
//     int? Skip,
//     string? SortBy
// ) : IRequest<CollectionResponse<SearchBookingResponse>>,
//     ISortableQuery,
//     IPageableQuery;
