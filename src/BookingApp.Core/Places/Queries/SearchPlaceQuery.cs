using BookingApp.Core.Commons.Queries;
using BookingApp.Core.Commons.ViewModels;
using BookingApp.Core.Places.ViewModels;
using MediatR;

namespace BookingApp.Core.Places.Queries;

public record SearchPlaceQuery(
    string? Search = null,
    int? Take = null,
    int? Skip = null,
    string? SortBy = null
) : IRequest<CollectionResponse<PlaceResponse>>,
    IPageableQuery,
    ISortableQuery;
