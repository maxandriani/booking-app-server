using BookingApp.Core.GuestContacts.Commands;
using FluentValidation;

namespace BookingApp.Core.GuestContacts.Validations;

public class DeleteGuestContactCmdValidator : AbstractValidator<DeleteGuestContactCmd>
{
    public DeleteGuestContactCmdValidator()
    {
        RuleFor(q => q.Id).NotEmpty().WithMessage("Você precisa informar um Contato existente.");
        RuleFor(q => q.GuestId).NotEmpty().WithMessage("Você precisa informar um Hóspede existente.");
    }
}