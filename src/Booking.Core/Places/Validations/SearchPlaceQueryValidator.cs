using Booking.Core.Places.Queries;
using FluentValidation;

namespace Booking.Core.Places.Validations;

public class SearchPlaceQueryValidator : AbstractValidator<SearchPlaceQuery>
{
    public SearchPlaceQueryValidator()
    {
    }
}