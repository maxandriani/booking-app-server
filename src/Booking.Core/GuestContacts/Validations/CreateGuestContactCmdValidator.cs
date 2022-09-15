using Booking.Core.GuestContacts.Commands;
using FluentValidation;

namespace Booking.Core.GuestContacts.Validations;

public class CreateGuestContactCmdValidator : AbstractValidator<CreateGuestContactCmd>
{
    public CreateGuestContactCmdValidator()
    {
        RuleFor(q => q.GuestId).NotEmpty().WithMessage("Você precisa informar um Hóspede válido.");
        RuleFor(q => q.Value).NotEmpty().WithMessage("Você precisa informar um contato.");
    }
}