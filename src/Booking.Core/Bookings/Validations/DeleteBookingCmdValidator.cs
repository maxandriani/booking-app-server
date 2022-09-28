using Booking.Core.Bookings.Commands;
using FluentValidation;

namespace Booking.Core.Bookings.Validations;

public class DeleteBookingCmdValidator : AbstractValidator<DeleteBookingCmd>
{
    public DeleteBookingCmdValidator()
    {
        RuleFor(q => q.Id).NotEmpty();
    }
}