using Booking.Core.Bookings.Commands;
using FluentValidation;

namespace Booking.Core.Bookings.Validations;

public class CreateBookingCmdValidator : AbstractValidator<CreateBookingCmd>
{
    public CreateBookingCmdValidator()
    {
        RuleFor(q => q.PlaceId).NotEmpty();
        RuleFor(q => q.CheckIn)
            .NotEmpty()
            .GreaterThanOrEqualTo(p => p.CheckOut);
        RuleFor(q => q.CheckOut)
            .NotEmpty()
            .LessThanOrEqualTo(p => p.CheckIn);
    }
}