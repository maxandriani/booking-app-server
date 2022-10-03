using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.Data;
using BookingApp.Core.GuestContacts.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.GuestContacts.Rules;

public class GuestContactShallReferenceExistingGuest :
    INotificationHandler<ValidateCreateGuestContactCmdRules>,
    INotificationHandler<ValidateUpdateGuestContactCmdRules>
{
    public string RuleName => "O hóspede informado não existe.";
    private readonly BookingDbContext _dbContext;

    public GuestContactShallReferenceExistingGuest(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public Task Handle(ValidateCreateGuestContactCmdRules notification, CancellationToken cancellationToken)
        => CheckIfGuestExists(notification.GuestContact.GuestId);

    public Task Handle(ValidateUpdateGuestContactCmdRules notification, CancellationToken cancellationToken)
        => CheckIfGuestExists(notification.GuestContact.GuestId);

    private async Task CheckIfGuestExists(Guid guestId)
    {
        var exists = await _dbContext.Guests.AnyAsync(q => q.Id == guestId);
        if (exists == false) throw new ResourceNotFoundException(RuleName);
    }
}

