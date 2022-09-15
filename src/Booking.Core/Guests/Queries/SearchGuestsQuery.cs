using Booking.Core.Commons.Queries;
using Booking.Core.Commons.ViewModels;
using Booking.Core.Guests.ViewModels;
using MediatR;

namespace Booking.Core.Guests.Queries;

/// <summary>
/// Consulta um intervalo de uma coleção de Hóspedes.
/// </summary>
public record SearchGuestsQuery(
    int? Take = default!,
    int? Skip = default!,
    string? SortBy = default!,
    string? Search = default!,
    bool? WithContacts = default!
) :
    ISortableQuery,
    IPageableQuery,
    IRequest<CollectionResponse<GuestWithContactsResponse>>;