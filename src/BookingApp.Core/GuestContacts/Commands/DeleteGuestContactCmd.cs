using MediatR;

namespace BookingApp.Core.GuestContacts.Commands;

/// <summary>
/// Remove um contato de um hóspede.
/// </summary>
public record DeleteGuestContactCmd(
    Guid Id,
    Guid GuestId) : IRequest;
