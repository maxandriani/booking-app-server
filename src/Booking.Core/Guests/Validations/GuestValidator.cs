using Booking.Core.GuestContacts.Validations;
using Booking.Core.Guests.Models;
using FluentValidation;

namespace Booking.Core.Guests.Validations;

public class GuestValidator : AbstractValidator<Guest>
{
    public static byte NameMinLength = 3;

    public GuestValidator()
    {
        RuleFor(p => p.Name)
            .NotEmpty()
            .MinimumLength(NameMinLength)
            .WithMessage($"O campo Nome deve conter pelo menos {NameMinLength} caracteres.");

        RuleForEach(q => q.Contacts)
            .SetValidator(new GuestContactValidator());
    }
}