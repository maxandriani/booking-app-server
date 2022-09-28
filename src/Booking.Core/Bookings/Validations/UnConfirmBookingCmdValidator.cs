using Booking.Core.Bookings.Commands;
using FluentValidation;

namespace Booking.Core.Bookings.Validations;

public class UnConfirmBookingCmdValidator : AbstractValidator<UnConfirmBookingCmd>
{
    public UnConfirmBookingCmdValidator()
    {
        RuleFor(q => q.BookingId).NotEmpty();
    }
}