using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using BookingApp.Core.GuestContacts.Models;
using BookingApp.Core.GuestContacts.Queries;
using BookingApp.Core.GuestContacts.ViewModels;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.GuestContacts;

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
