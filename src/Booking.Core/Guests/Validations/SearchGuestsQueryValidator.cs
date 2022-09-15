using Booking.Core.Guests.Queries;
using FluentValidation;

namespace Booking.Core.Guests.Validations;

public class SearchGuestsQueryValidator : AbstractValidator<SearchGuestsQuery>
{
    public SearchGuestsQueryValidator()
    {
    }
}