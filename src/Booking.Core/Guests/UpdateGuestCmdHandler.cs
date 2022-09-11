using Booking.Core.Commons.Exceptions;
using Booking.Core.Data;
using Booking.Core.Guests.Commands;
using Booking.Core.Guests.Models;
using Booking.Core.Guests.Responses;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Core.Guests;

public class UpdateGuestCmdHandler : IRequestHandler<UpdateGuestCmd, GuestResponse>
{
    private readonly BookingDbContext _dbContext;
    private readonly IValidator<Guest> _validator;

    public UpdateGuestCmdHandler(
        BookingDbContext dbContext,
        IValidator<Guest> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task<GuestResponse> Handle(UpdateGuestCmd request, CancellationToken cancellationToken)
    {
        if (request.Body == null) throw new ArgumentNullException(nameof(request.Body));
        if (request.Id.Equals(Guid.Empty)) throw new ArgumentOutOfRangeException(nameof(request.Id));
        var guest = await _dbContext.Guests.FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);
        if (guest == null) throw new ResourceNotFoundException(nameof(Guest));

        request.Body.MapTo(guest);
        _validator.ValidateAndThrow(guest);

        _dbContext.Update(guest);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new GuestResponse(guest);
    }
}
