using Booking.Core.GuestContacts.Queries;
using FluentValidation;

namespace Booking.Core.GuestContacts.Validations;

public class SearchGuestContactQueryValidator : AbstractValidator<SearchGuestContactQuery>
{
    public SearchGuestContactQueryValidator()
    {
        RuleFor(q => q.GuestId).NotEmpty().WithMessage("Você precisa informar um Hóspede existente.");
    }
}