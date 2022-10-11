using BookingApp.Core.Bookings.Commands;
using FluentValidation;

namespace BookingApp.Core.Bookings.Validations;

public class DeleteBookingGuestCmdValidator : AbstractValidator<DeleteBookingGuestCmd>
{
    public DeleteBookingGuestCmdValidator()
    {
        RuleFor(q => q.BookingId).NotEmpty();
        RuleFor(q => q.GuestId).NotEmpty();
    }
}