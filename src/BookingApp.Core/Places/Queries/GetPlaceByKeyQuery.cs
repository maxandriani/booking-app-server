using BookingApp.Core.Places.ViewModels;
using MediatR;

namespace BookingApp.Core.Guests.Queries;

public record GetPlaceByKeyQuery(
    Guid Id
) : IRequest<PlaceResponse>;