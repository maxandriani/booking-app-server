using BookingApp.Core.Places.Queries;
using FluentValidation;

namespace BookingApp.Core.Places.Validations;

public class SearchPlaceQueryValidator : AbstractValidator<SearchPlaceQuery>
{
    public SearchPlaceQueryValidator()
    {
    }
}