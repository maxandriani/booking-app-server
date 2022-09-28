using Booking.Core.Bookings.Queries;
using FluentValidation;

namespace Booking.Core.Bookings.Validations;

public class GetBookingByKeyQueryValidator : AbstractValidator<GetBookingByKeyQuery>
{
    public GetBookingByKeyQueryValidator()
    {
        RuleFor(q => q.Id).NotEmpty();
    }
}