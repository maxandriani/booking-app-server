using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using BookingApp.Core.Guests.Models;
using BookingApp.Core.Guests.Queries;
using BookingApp.Core.Guests.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Guests;

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