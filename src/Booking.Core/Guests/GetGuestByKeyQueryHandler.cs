using Booking.Core.Commons.Exceptions;
using Booking.Core.Data;
using Booking.Core.Guests.Models;
using Booking.Core.Guests.Queries;
using Booking.Core.Guests.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Core.Guests;

public class GetGuestByKeyQueryHandler : IRequestHandler<GetGuestByKeyQuery, GuestResponse>
{
    private readonly BookingDbContext _dbContext;

    public GetGuestByKeyQueryHandler(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<GuestResponse> Handle(GetGuestByKeyQuery request, CancellationToken cancellationToken)
    {
        if (request.Id.Equals(Guid.Empty)) throw new ArgumentOutOfRangeException(nameof(request.Id));
        var guest = await _dbContext.Guests.FirstOrDefaultAsync(q => q.Id == request.Id, cancellationToken);
        if (guest == null) throw new ResourceNotFoundException(nameof(Guest));
        return new GuestResponse(guest);
    }
}