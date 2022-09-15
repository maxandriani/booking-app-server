using Booking.Core.GuestContacts.Models;
using FluentValidation;

namespace Booking.Core.GuestContacts.Validations;

public class GuestContactValidator : AbstractValidator<GuestContact>
{
    public GuestContactValidator()
    {
        RuleFor(q => q.Value)
            .NotEmpty()
            .WithMessage($"Você precisa informar um valor para o contato.");

        RuleFor(q => q.GuestId)
            .NotEmpty()
            .WithMessage($"Você precisa informar um Hóspede válido.");
    }
}