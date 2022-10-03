using BookingApp.Core.Places.Commands;
using FluentValidation;

namespace BookingApp.Core.Places.Validations;

public class DeletePlaceCmdValidator : AbstractValidator<DeletePlaceCmd>
{
    public DeletePlaceCmdValidator()
    {
        RuleFor(p => p.Id)
            .NotEmpty().WithMessage("Você precisa informar um Local.");
    }
}