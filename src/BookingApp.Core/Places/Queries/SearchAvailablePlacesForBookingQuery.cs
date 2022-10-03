using BookingApp.Core.Commons.Queries;
using BookingApp.Core.Commons.ViewModels;
using BookingApp.Core.Places.ViewModels;
using MediatR;

namespace BookingApp.Core.Places.Queries;

public record SearchAvailablePlacesForBookingQuery(
    DateTime CheckIn,
    DateTime CheckOut,
    int? Take = null,
    int? Skip = null,
    string? SortBy = null
) : IRequest<CollectionResponse<PlaceResponse>>,
    ISortableQuery,
    IPageableQuery;
