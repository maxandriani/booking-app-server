using BookingApp.Core.GuestContacts.Commands;
using FluentValidation;

namespace BookingApp.Core.GuestContacts.Validations;

public class UpdateGuestContactCmdValidator : AbstractValidator<UpdateGuestContactCmd>
{
    public UpdateGuestContactCmdValidator()
    {
        RuleFor(q => q.Id).NotEmpty().WithMessage("Você precisa informar um Contato válido.");
        RuleFor(q => q.GuestId).NotEmpty().WithMessage("Você precisa informar um Hóspede válido.");
        RuleFor(q => q.Value).NotEmpty().WithMessage("Você precisa informar um contato.");
    }
}