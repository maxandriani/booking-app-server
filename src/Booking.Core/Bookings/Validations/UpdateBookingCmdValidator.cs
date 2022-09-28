using Booking.Core.Bookings.Commands;
using FluentValidation;

namespace Booking.Core.Bookings.Validations;

public class UpdateBookingCmdValidator : AbstractValidator<UpdateBookingCmd>
{
    public UpdateBookingCmdValidator()
    {
        RuleFor(q => q.Id).NotEmpty();
        RuleFor(q => q.PlaceId).NotEmpty();
        RuleFor(q => q.CheckIn).NotEmpty().LessThanOrEqualTo(q => q.CheckOut);
        RuleFor(q => q.CheckOut).NotEmpty().GreaterThanOrEqualTo(q => q.CheckIn);
    }
}