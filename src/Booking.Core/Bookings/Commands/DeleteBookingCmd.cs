using MediatR;

namespace Booking.Core.Bookings.Commands;

public record DeleteBookingCmd(
    Guid Id
) : IRequest;
