using Booking.Core.Bookings.Commands;
using FluentValidation;

namespace Booking.Core.Bookings.Validations;

public class SetPrimaryBookingGuestCmdValidator : AbstractValidator<SetPrimaryBookingGuestCmd>
{
    public SetPrimaryBookingGuestCmdValidator()
    {
        RuleFor(p => p.BookingId).NotEmpty();
        RuleFor(p => p.GuestId).NotEmpty();
    }
}