using MediatR;

namespace Booking.Core.Bookings.Events;

public record CheckingDeleteBookingCmd(
    Bookings.Models.Booking Booking
) : INotification;
