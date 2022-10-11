using BookingApp.Core.Places.Models;
using MediatR;

namespace BookingApp.Core.Places.Events;

public record ValidateDeletePlaceCmdRules(
    Place Place
) : INotification;