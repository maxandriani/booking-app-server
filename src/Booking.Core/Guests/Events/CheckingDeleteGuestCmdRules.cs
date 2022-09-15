using Booking.Core.Guests.Models;
using MediatR;

namespace Booking.Core.Guests.Events;

public record CheckingDeleteGuestCmdRules(
    Guest Guest
) : INotification;