using BookingApp.Core.Guests.Commands;
using FluentValidation;

namespace BookingApp.Core.Guests.Validations;

public class DeleteGuestCmdValidator : AbstractValidator<DeleteGuestCmd>
{
    public DeleteGuestCmdValidator()
    {
        RuleFor(q => q.Id).NotEmpty().WithMessage("Você precisa informar um Hóspede válido.");
    }
}