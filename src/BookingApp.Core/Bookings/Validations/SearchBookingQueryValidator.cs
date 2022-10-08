// using BookingApp.Core.Bookings.Queries;
// using FluentValidation;

// namespace BookingApp.Core.Bookings.Validations;

// public class SearchBookingQueryValidator : AbstractValidator<SearchBookingsQuery>
// {
//     public SearchBookingQueryValidator()
//     {
//         RuleFor(q => q.ByPlace).Must(q => q != null && q != Guid.Empty).WithMessage("Você precisa informar um local válido.");
//         RuleFor(q => q.CheckIn).LessThanOrEqualTo(q => q.CheckOut);
//         RuleFor(q => q.CheckOut).GreaterThanOrEqualTo(q => q.CheckIn);
//     }
// }