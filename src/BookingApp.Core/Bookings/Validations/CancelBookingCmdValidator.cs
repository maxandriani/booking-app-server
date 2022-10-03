using BookingApp.Core.Bookings.Commands;
using FluentValidation;

namespace BookingApp.Core.Bookings.Validations;

public class CancelBookingCmdValidator : AbstractValidator<CancelBookingCmd>
{
    public CancelBookingCmdValidator()
    {
        RuleFor(q => q.BookingId).NotEmpty();
        RuleFor(q => q.GuestId).NotEmpty();
    }
}