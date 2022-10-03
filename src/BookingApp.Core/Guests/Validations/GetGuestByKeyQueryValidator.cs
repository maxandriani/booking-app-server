using BookingApp.Core.Guests.Queries;
using FluentValidation;

namespace BookingApp.Core.Guests.Validations;

public class GetGuestByKeyQueryValidator : AbstractValidator<GetGuestByKeyQuery>
{
    public GetGuestByKeyQueryValidator()
    {
        RuleFor(q => q.Id).NotEmpty().WithMessage("Você precisa informar um Hóspede válido.");
    }
}