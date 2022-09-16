using Booking.Core.Places.ViewModels;
using MediatR;

namespace Booking.Core.Guests.Queries;

public record GetPlaceByKeyQuery(
    Guid Id
) : IRequest<PlaceResponse>;