using Booking.Core.Bookings.Commands;
using FluentValidation;

namespace Booking.Core.Bookings.Validations;

public class AddBookingGuestCmdValidator : AbstractValidator<AddBookingGuestCmd>
{
    public AddBookingGuestCmdValidator()
    {
        RuleFor(q => q.BookingId).NotEmpty();
        RuleFor(q => q.GuestId).NotEmpty();
    }
}