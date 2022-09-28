using MediatR;

namespace Booking.Core.Bookings.Events;

public record CheckingCreateBookingCmd(
    Bookings.Models.Booking booking
) : INotification;
