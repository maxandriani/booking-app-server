using Booking.Core.GuestContacts.Queries;
using FluentValidation;

namespace Booking.Core.GuestContacts.Validations;

public class GetGuestContactByKeyQueryValidator : AbstractValidator<GetGuestContactByKeyQuery>
{
    public GetGuestContactByKeyQueryValidator()
    {
        RuleFor(q => q.Id).NotEmpty().WithMessage("Você precisa informar um Contato existente.");
        RuleFor(q => q.GuestId).NotEmpty().WithMessage("Você precisa informar um Hóspede existente.");
    }
}