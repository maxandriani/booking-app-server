using MediatR;

namespace Booking.Core.Bookings.Commands;

public record ConfirmBookingCmd(
    Guid BookingId
) : IRequest;
