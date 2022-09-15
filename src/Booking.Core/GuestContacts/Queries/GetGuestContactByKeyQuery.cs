using Booking.Core.GuestContacts.ViewModels;
using MediatR;

namespace Booking.Core.GuestContacts.Queries;

public record GetGuestContactByKeyQuery(
    Guid Id,
    Guid GuestId) : IRequest<GuestContactResponse>;
