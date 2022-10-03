using BookingApp.Core.Places.Commands;
using FluentValidation;

namespace BookingApp.Core.Places.Validations;

public class UpdatePlaceCmdValidator : AbstractValidator<UpdatePlaceCmd>
{
    public UpdatePlaceCmdValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("Você precisa informar um Local.");
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("O campo nome precisa ser informado.")
            .MinimumLength(3).WithMessage("O campo nome precisa ter ao menos 3 caracteres.");
        RuleFor(p => p.Address)
            .NotEmpty().WithMessage("O campo Endereço precisa ser informado.")
            .MinimumLength(3).WithMessage("O campo endereço precisa ter ao menos 3 catacteres.");
    }
}