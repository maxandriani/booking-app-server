using Booking.Core.Data;
using Booking.Core.Guests.Commands;
using Booking.Core.Guests.Models;
using Booking.Core.Guests.Responses;
using FluentValidation;
using MediatR;

namespace Booking.Core.Guests;

public class CreateGuestCmdHandler : IRequestHandler<CreateGuestCmd, GuestResponse>
{
    private readonly BookingDbContext _dbContext;
    private readonly IValidator<Guest> _validator;

    public CreateGuestCmdHandler(
        BookingDbContext dbContext,
        IValidator<Guest> validator)
    {
        _dbContext = dbContext;
        _validator = validator;
    }

    public async Task<GuestResponse> Handle(CreateGuestCmd request, CancellationToken cancellationToken)
    {
        if (request.Body == null) throw new ArgumentNullException(nameof(request.Body));

        var guest = new Guest();
        request.Body.MapTo(guest);
        _validator.ValidateAndThrow(guest);

        _dbContext.Add(guest);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new GuestResponse(guest);
    }
}
