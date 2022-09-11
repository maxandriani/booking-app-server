using Booking.Core.Commons.Responses;
using Booking.Core.Guests.Responses;
using MediatR;

namespace Booking.Core.Guests.Queries;

/// <summary>
/// Consulta um intervalo de uma coleção de Hóspedes.
/// </summary>
public class SearchGuestsQuery : IRequest<CollectionResponse<GuestResponse>>
{
    public int? Take { get; set; }
    public int? Skip { get; set; }
    public string? Sort { get; set; }
    public string? Search { get; set; }
}