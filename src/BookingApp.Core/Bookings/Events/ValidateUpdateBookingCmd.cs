using MediatR;

namespace BookingApp.Core.Bookings.Events;

public record ValidateUpdateBookingCmd(
    Bookings.Models.Booking Booking
) : INotification;
