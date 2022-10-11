using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.Data;
using BookingApp.Core.Places.Events;
using BookingApp.Core.Places.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Places.Rules;

public class PlaceNameShallBeUnique :
    INotificationHandler<ValidateCreatePlaceCmdRules>,
    INotificationHandler<ValidateUpdatePlaceCmdRules>
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

    public Task Handle(ValidateUpdatePlaceCmdRules notification, CancellationToken cancellationToken)
        => ValidateUniqueNameRule(notification.Place, cancellationToken);

    public Task Handle(ValidateCreatePlaceCmdRules notification, CancellationToken cancellationToken)
        => ValidateUniqueNameRule(notification.Place, cancellationToken);
}
