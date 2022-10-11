using BookingApp.Core.Bookings.Commands;
using FluentValidation;

namespace BookingApp.Core.Bookings.Validations;

public class DeleteBookingCmdValidator : AbstractValidator<DeleteBookingCmd>
{
    public DeleteBookingCmdValidator()
    {
        RuleFor(q => q.Id).NotEmpty();
    }
}