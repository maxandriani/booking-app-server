using MediatR;

namespace BookingApp.Core.Places.Commands;

public record DeletePlaceCmd(
    Guid Id
) : IRequest;