using BookingApp.Core.Bookings.ViewModels;
using MediatR;

namespace BookingApp.Core.Bookings.Queries;

public record GetBookingByKeyQuery(
    Guid Id
) : IRequest<BookingResponse>;
