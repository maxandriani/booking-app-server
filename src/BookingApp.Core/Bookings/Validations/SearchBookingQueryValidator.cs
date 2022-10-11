using BookingApp.Core.Bookings.Queries;
using FluentValidation;

namespace BookingApp.Core.Bookings.Validations;

public class SearchBookingQueryValidator : AbstractValidator<SearchBookingsQuery>
{
    public SearchBookingQueryValidator()
    {
        RuleFor(q => q.ByPlace).Must(q => q == null || q != null && q != Guid.Empty).WithMessage("Você precisa informar um local válido.");
    }
}