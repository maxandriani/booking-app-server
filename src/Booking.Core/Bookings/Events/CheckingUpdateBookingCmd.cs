using MediatR;

namespace Booking.Core.Bookings.Events;

public record CheckingUpdateBookingCmd(
    Bookings.Models.Booking Booking
) : INotification;
