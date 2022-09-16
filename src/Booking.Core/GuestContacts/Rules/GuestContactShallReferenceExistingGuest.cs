using Booking.Core.Commons.Exceptions;
using Booking.Core.Data;
using Booking.Core.GuestContacts.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Core.GuestContacts.Rules;

public class GuestContactShallReferenceExistingGuest :
    INotificationHandler<CheckingCreateGuestContactCmdRules>,
    INotificationHandler<CheckingUpdateGuestContactCmdRules>
{
    public string RuleName => "O hóspede informado não existe.";
    private readonly BookingDbContext _dbContext;

    public GuestContactShallReferenceExistingGuest(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task Handle(CheckingCreateGuestContactCmdRules notification, CancellationToken cancellationToken)
        => CheckIfGuestExists(notification.GuestContact.GuestId);

    public Task Handle(CheckingUpdateGuestContactCmdRules notification, CancellationToken cancellationToken)
        => CheckIfGuestExists(notification.GuestContact.GuestId);

    private async Task CheckIfGuestExists(Guid guestId)
    {
        var exists = await _dbContext.Guests.AnyAsync(q => q.Id == guestId);
        if (exists == false) throw new ResourceNotFoundException(RuleName);
    }
}

