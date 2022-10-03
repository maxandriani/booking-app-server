using MediatR;

namespace BookingApp.Core.Bookings.Commands;

public record CancelBookingCmd(
    Guid BookingId
) : IRequest;