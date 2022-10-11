using MediatR;

namespace BookingApp.Core.Bookings.Events;

public record ValidateDeleteBookingCmd(
    Bookings.Models.Booking Booking
) : INotification;
