using Booking.Core.Guests.Models;
using MediatR;

namespace Booking.Core.Guests.Events;

public record CheckingUpdateGuestCmdRules(
    Guest Guest
) : INotification;