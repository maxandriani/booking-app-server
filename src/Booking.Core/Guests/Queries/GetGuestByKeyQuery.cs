using Booking.Core.Guests.ViewModels;
using MediatR;

namespace Booking.Core.Guests.Queries;

public record GetGuestByKeyQuery(
    Guid Id
) : IRequest<GuestWithContactsResponse>;