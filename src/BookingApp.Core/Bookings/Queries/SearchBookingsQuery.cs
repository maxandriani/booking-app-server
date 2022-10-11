using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Bookings.ViewModels;
using BookingApp.Core.Commons.Queries;
using BookingApp.Core.Commons.ViewModels;
using MediatR;

namespace BookingApp.Core.Bookings.Queries;

public record SearchBookingsQuery(
    Guid? ByPlace = null,
    DateTime? Date = null,
    BookingStatusEnum? Status = null,
    string? Search = null,
    int? Take = null,
    int? Skip = null,
    string? SortBy = null
) : IRequest<CollectionResponse<SearchBookingResponse>>,
    ISortableQuery,
    IPageableQuery;
