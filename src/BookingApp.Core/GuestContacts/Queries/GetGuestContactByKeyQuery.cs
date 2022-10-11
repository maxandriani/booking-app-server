using BookingApp.Core.GuestContacts.ViewModels;
using MediatR;

namespace BookingApp.Core.GuestContacts.Queries;

public record GetGuestContactByKeyQuery(
    Guid Id,
    Guid GuestId) : IRequest<GuestContactResponse>;
