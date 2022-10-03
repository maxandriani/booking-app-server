using BookingApp.Core.Bookings.Commands;
using FluentValidation;

namespace BookingApp.Core.Bookings.Validations;

public class AddBookingGuestCmdValidator : AbstractValidator<AddBookingGuestCmd>
{
    public AddBookingGuestCmdValidator()
    {
        RuleFor(q => q.BookingId).NotEmpty();
        RuleFor(q => q.GuestId).NotEmpty();
    }
}