using BookingApp.Core.Places.Commands;
using FluentValidation;

namespace BookingApp.Core.Places.Validations;

public class CreatePlaceCmdValidator : AbstractValidator<CreatePlaceCmd>
{
    public CreatePlaceCmdValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty().WithMessage("O campo nome precisa ser informado.")
            .MinimumLength(3).WithMessage("O campo nome precisa ter ao menos 3 catacteres.");
        RuleFor(p => p.Address)
            .NotEmpty().WithMessage("O campo Endereço precisa ser informado.")
            .MinimumLength(3).WithMessage("O campo endereço precisa ter ao menos 3 catacteres.");
    }
}