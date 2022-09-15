using Booking.Core.GuestContacts.Models;
using MediatR;

namespace Booking.Core.GuestContacts.Events;

public record CheckingDeleteGuestContactCmdRules(GuestContact GuestContact) : INotification;
