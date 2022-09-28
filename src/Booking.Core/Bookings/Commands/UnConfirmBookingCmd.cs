using MediatR;

namespace Booking.Core.Bookings.Commands;

public record UnConfirmBookingCmd(
    Guid BookingId
) : IRequest;
