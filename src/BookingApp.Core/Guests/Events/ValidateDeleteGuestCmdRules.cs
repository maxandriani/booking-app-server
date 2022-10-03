using BookingApp.Core.Guests.Models;
using MediatR;

namespace BookingApp.Core.Guests.Events;

public record ValidateDeleteGuestCmdRules(
    Guest Guest
) : INotification;