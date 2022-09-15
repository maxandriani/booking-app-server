using Booking.Core.GuestContacts.Models;
using MediatR;

namespace Booking.Core.GuestContacts.Events;

public record CheckingCreateGuestContactCmdRules(GuestContact GuestContact) : INotification;
