using MediatR;

namespace BookingApp.Core.Bookings.Events;

public record ValidateCreateBookingCmd(
    Bookings.Models.Booking booking
) : INotification;
