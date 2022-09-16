using Booking.Core.Commons.Exceptions;
using Booking.Core.Data;
using Booking.Core.Places.Events;
using Booking.Core.Places.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Core.Places.Rules;

public class PlaceNameShallBeUnique :
    INotificationHandler<CheckingCreatePlaceCmdRules>,
    INotificationHandler<CheckingUpdatePlaceCmdRules>
{
    private readonly BookingDbContext _dbContext;

    public PlaceNameShallBeUnique(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    private async Task ValidateUniqueNameRule(Place place, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.Places.AnyAsync(q => q.Name.Equals(place.Name) && q.Id != place.Id);
        if (exists) throw new ResourceAlreadyExistsException(typeof(Place).Name, place.Name);
    }

    public Task Handle(CheckingUpdatePlaceCmdRules notification, CancellationToken cancellationToken)
        => ValidateUniqueNameRule(notification.Place, cancellationToken);

    public Task Handle(CheckingCreatePlaceCmdRules notification, CancellationToken cancellationToken)
        => ValidateUniqueNameRule(notification.Place, cancellationToken);
}
