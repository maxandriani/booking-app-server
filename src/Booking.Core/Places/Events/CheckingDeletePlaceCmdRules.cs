using Booking.Core.Places.Models;
using MediatR;

namespace Booking.Core.Places.Events;

public record CheckingDeletePlaceCmdRules(
    Place Place
) : INotification;