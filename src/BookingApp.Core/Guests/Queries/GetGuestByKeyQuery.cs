using BookingApp.Core.Guests.ViewModels;
using MediatR;

namespace BookingApp.Core.Guests.Queries;

public record GetGuestByKeyQuery(
    Guid Id
) : IRequest<GuestWithContactsResponse>;