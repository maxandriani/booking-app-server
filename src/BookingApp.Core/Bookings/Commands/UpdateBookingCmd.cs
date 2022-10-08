using BookingApp.Core.Bookings.ViewModels;
using MediatR;

namespace BookingApp.Core.Bookings.Commands;

public record UpdateBookingCmd(
    Guid Id,
    Guid PlaceId,
    DateTime CheckIn,
    DateTime CheckOut,
    string? Description
) : IRequest<BookingResponse>;
