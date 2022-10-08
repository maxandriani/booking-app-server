using BookingApp.Core.Bookings.Commands;
using FluentValidation;

namespace BookingApp.Core.Bookings.Validations;

public class SetPrimaryBookingGuestCmdValidator : AbstractValidator<SetPrimaryBookingGuestCmd>
{
    public SetPrimaryBookingGuestCmdValidator()
    {
        RuleFor(p => p.BookingId).NotEmpty();
        RuleFor(p => p.GuestId).NotEmpty();
    }
}