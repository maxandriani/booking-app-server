using MediatR;

namespace BookingApp.Core.Bookings.Commands;

public record DeleteBookingCmd(
    Guid Id
) : IRequest;
