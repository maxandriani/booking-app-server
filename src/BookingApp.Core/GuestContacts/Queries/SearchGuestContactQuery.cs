using BookingApp.Core.Commons.Queries;
using BookingApp.Core.Commons.ViewModels;
using BookingApp.Core.GuestContacts.ViewModels;
using MediatR;

namespace BookingApp.Core.GuestContacts.Queries;

public record SearchGuestContactQuery(
    Guid GuestId,
    int? Take = null,
    int? Skip = null,
    string? SortBy = null,
    string? Search = null
) : IRequest<CollectionResponse<GuestContactResponse>>,
    ISortableQuery,
    IPageableQuery;