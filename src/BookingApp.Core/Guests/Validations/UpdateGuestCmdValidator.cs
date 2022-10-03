using BookingApp.Core.Guests.Commands;
using FluentValidation;

namespace BookingApp.Core.Guests.Validations;

public class UpdateGuestCmdValidator : AbstractValidator<UpdateGuestCmd>
{
    public UpdateGuestCmdValidator()
    {
        RuleFor(q => q.Id).NotEmpty().WithMessage("Você precisa informar um Hóspede válido.");
        RuleFor(p => p.Name).NotEmpty().MinimumLength(3).WithMessage("O nome do hóspede precisa ter pelo menos 3 caracteres.");
    }
}