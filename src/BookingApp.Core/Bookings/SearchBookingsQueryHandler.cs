using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Bookings.Queries;
using BookingApp.Core.Bookings.ViewModels;
using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Bookings;

public class SearchBookingsQueryHandler : SearchEntityQueryHandlerBase<
    BookingDbContext,
    Booking,
    SearchBookingsQuery,
    SearchBookingResponse>
{
    public SearchBookingsQueryHandler(
        BookingDbContext dbContext,
        IValidator<SearchBookingsQuery> validator) : base(dbContext, validator)
    {
    }

    protected override IQueryable<Booking> ApplyDefaultSorting(IQueryable<Booking> query)
        => query.OrderBy(q => q.CheckIn);

    protected override IQueryable<Booking> CreateSearchQuery(SearchBookingsQuery request)
    {
        IQueryable<Booking> query = _dbContext.Bookings
            .Include(q => q.Place)
            .Include(q => q.Guests.Where(q => q.IsPrincipal))
                .ThenInclude(q => q.Guest)
                    .ThenInclude(q => q!.Contacts);

        if (request.ByPlace != null)
            query = query.Where(q => q.PlaceId == request.ByPlace.Value);
        
        if (request.Date != null)
            query = query.Where(q => q.CheckIn.Date <= request.Date.Value.Date && q.CheckOut.Date >= request.Date.Value);
        
        if (request.Status != null)
            query = query.Where(q => q.Status == request.Status.Value);

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(q =>
                q.Description != null && q.Description.ToLower().StartsWith(request.Search.ToLower())
                || q.Guests.Any(q => q.Guest!.Name.ToLower().StartsWith(request.Search.ToLower())));

        return query;
    }

    protected override IQueryable<SearchBookingResponse> MapToResponse(IQueryable<Booking> query)
        => query.Select(x => new SearchBookingResponse(x));
}