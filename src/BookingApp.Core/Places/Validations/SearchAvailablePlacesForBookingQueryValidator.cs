using BookingApp.Core.Places.Queries;
using FluentValidation;

namespace BookingApp.Core.Places.Validations;

public class SearchAvailablePlacesForBookingQueryValidator : AbstractValidator<SearchAvailablePlacesForBookingQuery>
{
    public SearchAvailablePlacesForBookingQueryValidator()
    {
        RuleFor(q => q.CheckIn).NotEmpty().LessThanOrEqualTo(q => q.CheckOut);
        RuleFor(q => q.CheckOut).NotEmpty().GreaterThanOrEqualTo(q => q.CheckIn);
    }
}