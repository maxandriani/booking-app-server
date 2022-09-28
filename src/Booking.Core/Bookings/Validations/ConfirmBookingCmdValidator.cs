using Booking.Core.Bookings.Commands;
using FluentValidation;

namespace Booking.Core.Bookings.Validations;

public class ConfirmBookingCmdValidator : AbstractValidator<ConfirmBookingCmd>
{
    public ConfirmBookingCmdValidator()
    {
        RuleFor(q => q.BookingId).NotEmpty();
    }
}