using Booking.Core.GuestContacts.Models;
using MediatR;

namespace Booking.Core.GuestContacts.Events;

public record CheckingUpdateGuestContactCmdRules(GuestContact GuestContact) : INotification;

