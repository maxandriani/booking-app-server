// using BookingApp.Core.Bookings.Commands;
// using FluentValidation;

// namespace BookingApp.Core.Bookings.Validations;

// public class UpdateBookingCmdValidator : AbstractValidator<UpdateBookingCmd>
// {
//     public UpdateBookingCmdValidator()
//     {
//         RuleFor(q => q.Id).NotEmpty();
//         RuleFor(q => q.PlaceId).NotEmpty();
//         RuleFor(q => q.CheckIn).NotEmpty().LessThanOrEqualTo(q => q.CheckOut);
//         RuleFor(q => q.CheckOut).NotEmpty().GreaterThanOrEqualTo(q => q.CheckIn);
//     }
// }