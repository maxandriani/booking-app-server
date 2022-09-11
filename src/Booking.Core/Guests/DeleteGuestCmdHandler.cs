using Booking.Core.Commons.Exceptions;
using Booking.Core.Data;
using Booking.Core.Guests.Commands;
using Booking.Core.Guests.Models;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Core.Guests;

public class DeleteGuestCmdHandler : IRequestHandler<DeleteGuestCmd>
{
    private readonly BookingDbContext _dbContext;
    private readonly IValidator<Guest> _validator;

    public DeleteGuestCmdHandler(
        BookingDbContext dbContext,
        IValidator<Guest> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task<Unit> Handle(DeleteGuestCmd request, CancellationToken cancellationToken)
    {
        if (request.Id.Equals(Guid.Empty)) throw new ArgumentOutOfRangeException(nameof(request.Id));
        var guest = await _dbContext.Guests.FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);
        if (guest == null) throw new ResourceNotFoundException(nameof(Guest));

        _dbContext.Remove(guest);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}
