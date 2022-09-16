using Booking.Core.Commons.Queries;
using Booking.Core.Commons.ViewModels;
using Booking.Core.Places.ViewModels;
using MediatR;

namespace Booking.Core.Places.Queries;

public record SearchPlaceQuery(
    string? Search = null,
    int? Take = null,
    int? Skip = null,
    string? SortBy = null
) : IRequest<CollectionResponse<PlaceResponse>>,
    IPageableQuery,
    ISortableQuery;
