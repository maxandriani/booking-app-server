using Booking.Core.Bookings.Commands;
using FluentValidation;

namespace Booking.Core.Bookings.Validations;

public class CancelBookingCmdValidator : AbstractValidator<CancelBookingCmd>
{
    public CancelBookingCmdValidator()
    {
        RuleFor(q => q.BookingId).NotEmpty();
        RuleFor(q => q.GuestId).NotEmpty();
    }
}