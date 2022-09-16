using MediatR;

namespace Booking.Core.Places.Commands;

public record DeletePlaceCmd(
    Guid Id
) : IRequest;