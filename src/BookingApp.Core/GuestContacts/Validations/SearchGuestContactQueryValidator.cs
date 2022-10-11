using BookingApp.Core.GuestContacts.Queries;
using FluentValidation;

namespace BookingApp.Core.GuestContacts.Validations;

public class SearchGuestContactQueryValidator : AbstractValidator<SearchGuestContactQuery>
{
    public SearchGuestContactQueryValidator()
    {
        RuleFor(q => q.GuestId).NotEmpty().WithMessage("Você precisa informar um Hóspede existente.");
    }
}