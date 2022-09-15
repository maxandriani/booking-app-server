using Booking.Core.Guests.Commands;
using FluentValidation;

namespace Booking.Core.Guests.Validations;

public class CreateGuestWithContactsCmdValidator : AbstractValidator<CreateGuestWithContactsCmd>
{
    public CreateGuestWithContactsCmdValidator()
    {
        RuleFor(p => p.Name).NotEmpty().MinimumLength(3).WithMessage("O nome do hóspede precisa ter pelo menos 3 caracteres.");
        RuleForEach(p => p.Contacts)
            .ChildRules(p =>
            {
                p.RuleFor(p => p.Value).NotEmpty().WithMessage("Você precisa informar um contato.");
            });
    }
}