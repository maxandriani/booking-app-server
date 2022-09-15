using Booking.Core.Commons.Exceptions;
using Booking.Core.Commons.Handlers;
using Booking.Core.Data;
using Booking.Core.Guests.Models;
using Booking.Core.Guests.Queries;
using Booking.Core.Guests.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Booking.Core.Guests;

public class GetGuestByKeyQueryHandler :
    GetEntityByKeyQueryHandlerBase<BookingDbContext, Guest, GetGuestByKeyQuery, GuestWithContactsResponse>
{
    public GetGuestByKeyQueryHandler(
        BookingDbContext dbContext,
        IValidator<GetGuestByKeyQuery> validator) : base(dbContext, validator)
    {
    }

    protected override Task<Guest?> GetByKeyAsync(GetGuestByKeyQuery request)
        => _dbContext.Guests
            .Include(q => q.Contacts)
            .FirstOrDefaultAsync(q => q.Id == request.Id);

    protected override GuestWithContactsResponse MapToResponse(Guest entity)
        => new GuestWithContactsResponse(entity);
}