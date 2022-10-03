using BookingApp.Core.GuestContacts.Queries;
using FluentValidation;

namespace BookingApp.Core.GuestContacts.Validations;

public class GetGuestContactByKeyQueryValidator : AbstractValidator<GetGuestContactByKeyQuery>
{
    public GetGuestContactByKeyQueryValidator()
    {
        RuleFor(q => q.Id).NotEmpty().WithMessage("Você precisa informar um Contato existente.");
        RuleFor(q => q.GuestId).NotEmpty().WithMessage("Você precisa informar um Hóspede existente.");
    }
}