using Booking.Core.Commons.Handlers;
using Booking.Core.Commons.ViewModels;
using Booking.Core.Data;
using Booking.Core.Guests.Models;
using Booking.Core.Guests.Queries;
using Booking.Core.Guests.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Booking.Core.Guests;

public class SearchGuestsQueryHandler :
    SearchEntityQueryHandlerBase<BookingDbContext, Guest, SearchGuestsQuery, GuestWithContactsResponse>
{
    public SearchGuestsQueryHandler(
        BookingDbContext dbContext,
        IValidator<SearchGuestsQuery> validator) : base(dbContext, validator)
    {
    }

    protected override IQueryable<Guest> ApplyDefaultSorting(IQueryable<Guest> query)
        => query.OrderBy(q => q.Name);

    protected override IQueryable<Guest> CreateSearchQuery(SearchGuestsQuery request)
    {
        var query = _dbContext.Guests.AsQueryable();

        if (request?.WithContacts == true)
            query = query.Include(q => q.Contacts);

        if (!string.IsNullOrWhiteSpace(request?.Search))
            query = query.Where(q => q.Name.ToLower().StartsWith(request.Search.ToLower()));

        return query;
    }

    protected override IQueryable<GuestWithContactsResponse> MapToResponse(IQueryable<Guest> query)
        => query.Select(entity => new GuestWithContactsResponse(entity));
}
