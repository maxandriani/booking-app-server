using MediatR;

namespace BookingApp.Core.Bookings.Commands;

public record UnConfirmBookingCmd(
    Guid BookingId
) : IRequest;
