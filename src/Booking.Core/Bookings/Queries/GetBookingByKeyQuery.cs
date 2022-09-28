using Booking.Core.Bookings.ViewModels;
using MediatR;

namespace Booking.Core.Bookings.Queries;

public record GetBookingByKeyQuery(
    Guid Id
) : IRequest<BookingResponse>;
