using BookingApp.Core.GuestContacts.Models;
using MediatR;

namespace BookingApp.Core.GuestContacts.Events;

public record ValidateUpdateGuestContactCmdRules(GuestContact GuestContact) : INotification;

