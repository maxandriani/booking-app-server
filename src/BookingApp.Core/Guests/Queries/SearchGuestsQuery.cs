using BookingApp.Core.Commons.Queries;
using BookingApp.Core.Commons.ViewModels;
using BookingApp.Core.Guests.ViewModels;
using MediatR;

namespace BookingApp.Core.Guests.Queries;

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