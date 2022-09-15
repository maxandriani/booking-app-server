using Booking.Core.Commons.Queries;
using Booking.Core.Commons.ViewModels;
using Booking.Core.GuestContacts.ViewModels;
using MediatR;

namespace Booking.Core.GuestContacts.Queries;

public record SearchGuestContactQuery (
    Guid GuestId,
    int? Take = null,
    int? Skip = null,
    string? SortBy = null,
    string? Search = null
) : IRequest<CollectionResponse<GuestContactResponse>>,
    ISortableQuery,
    IPageableQuery;