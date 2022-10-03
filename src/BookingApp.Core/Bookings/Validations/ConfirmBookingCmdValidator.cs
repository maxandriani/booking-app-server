using BookingApp.Core.Bookings.Commands;
using FluentValidation;

namespace BookingApp.Core.Bookings.Validations;

public class ConfirmBookingCmdValidator : AbstractValidator<ConfirmBookingCmd>
{
    public ConfirmBookingCmdValidator()
    {
        RuleFor(q => q.BookingId).NotEmpty();
    }
}