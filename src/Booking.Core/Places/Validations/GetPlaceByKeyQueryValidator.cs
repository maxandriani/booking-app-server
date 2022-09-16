using Booking.Core.Guests.Queries;
using FluentValidation;

namespace Booking.Core.Places.Validations;

public class GetPlaceByKeyQueryValidator : AbstractValidator<GetPlaceByKeyQuery>
{
    public GetPlaceByKeyQueryValidator()
    {
        RuleFor(q => q.Id).NotEmpty().WithMessage("Você precisa informar um Local.");
    }
}