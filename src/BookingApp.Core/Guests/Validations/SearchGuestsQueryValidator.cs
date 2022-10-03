using BookingApp.Core.Guests.Queries;
using FluentValidation;

namespace BookingApp.Core.Guests.Validations;

public class SearchGuestsQueryValidator : AbstractValidator<SearchGuestsQuery>
{
    public SearchGuestsQueryValidator()
    {
    }
}