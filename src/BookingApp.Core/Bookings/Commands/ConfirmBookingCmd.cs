using MediatR;

namespace BookingApp.Core.Bookings.Commands;

public record ConfirmBookingCmd(
    Guid BookingId
) : IRequest;
