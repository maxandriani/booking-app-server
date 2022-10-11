using BookingApp.Core.Guests.Models;
using MediatR;

namespace BookingApp.Core.Guests.Events;

public record ValidateCreateGuestCmdRules(
    Guest Guest
) : INotification;