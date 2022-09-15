using Booking.Core.Commons.Handlers;
using Booking.Core.Data;
using Booking.Core.GuestContacts.Models;
using Booking.Core.GuestContacts.Queries;
using Booking.Core.GuestContacts.ViewModels;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Booking.Core.GuestContacts;

public class GetGuestContactByKeyQueryHandler :
    GetEntityByKeyQueryHandlerBase<BookingDbContext, GuestContact, GetGuestContactByKeyQuery, GuestContactResponse>
{
    public GetGuestContactByKeyQueryHandler(
        BookingDbContext dbContext,
        IValidator<GetGuestContactByKeyQuery> validator) : base(dbContext, validator)
    {
    }

    protected override Task<GuestContact?> GetByKeyAsync(GetGuestContactByKeyQuery request)
        => _dbContext.GuestContacts.FirstOrDefaultAsync(q => q.Id == request.Id && q.GuestId == request.GuestId);

    protected override GuestContactResponse MapToResponse(GuestContact entity)
        => new GuestContactResponse(entity);
}
