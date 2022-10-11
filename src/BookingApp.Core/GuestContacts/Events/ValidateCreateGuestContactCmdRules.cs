using BookingApp.Core.GuestContacts.Models;
using MediatR;

namespace BookingApp.Core.GuestContacts.Events;

public record ValidateCreateGuestContactCmdRules(GuestContact GuestContact) : INotification;
