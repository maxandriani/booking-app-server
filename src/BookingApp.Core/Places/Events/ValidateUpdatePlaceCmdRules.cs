using BookingApp.Core.Places.Models;
using MediatR;

namespace BookingApp.Core.Places.Events;

public record ValidateUpdatePlaceCmdRules(
    Place Place
) : INotification;