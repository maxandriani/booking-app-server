using Booking.Core.Guests.Responses;
using MediatR;

namespace Booking.Core.Guests.Queries;

public class GetGuestByKeyQuery : IRequest<GuestResponse>
{
    public Guid Id { get; set; }

    public GetGuestByKeyQuery(Guid id)
    {
        Id = id;
    }
}