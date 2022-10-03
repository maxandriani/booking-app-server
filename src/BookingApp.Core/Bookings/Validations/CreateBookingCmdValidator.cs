using BookingApp.Core.Bookings.Commands;
using FluentValidation;

namespace BookingApp.Core.Bookings.Validations;

public class CreateBookingCmdValidator : AbstractValidator<CreateBookingCmd>
{
    public CreateBookingCmdValidator()
    {
        RuleFor(q => q.PlaceId).NotEmpty();
        RuleFor(q => q.CheckIn)
            .NotEmpty()
            .LessThan(p => p.CheckOut.Date);
        RuleFor(q => q.CheckOut)
            .NotEmpty()
            .GreaterThan(p => p.CheckIn.Date);
    }
}