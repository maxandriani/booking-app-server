using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using BookingApp.Core.GuestContacts.Models;
using BookingApp.Core.GuestContacts.Queries;
using BookingApp.Core.GuestContacts.ViewModels;
using FluentValidation;

namespace BookingApp.Core.GuestContacts;

public class SearchGuestContactQueryHandler :
    SearchEntityQueryHandlerBase<BookingDbContext, GuestContact, SearchGuestContactQuery, GuestContactResponse>
{
    public SearchGuestContactQueryHandler(
        BookingDbContext dbContext,
        IValidator<SearchGuestContactQuery> validator) : base(dbContext, validator)
    {
    }

    protected override IQueryable<GuestContact> ApplyDefaultSorting(IQueryable<GuestContact> query)
        => query.OrderBy(q => q.Value);

    protected override IQueryable<GuestContact> CreateSearchQuery(SearchGuestContactQuery request)
    {
        var query = _dbContext.GuestContacts.AsQueryable().Where(q => q.GuestId == request.GuestId);

        if (!string.IsNullOrWhiteSpace(request?.Search))
            query = query.Where(q => q.Value.ToLower().StartsWith(request.Search.ToLower()));

        return query;
    }

    protected override IQueryable<GuestContactResponse> MapToResponse(IQueryable<GuestContact> query)
        => query.Select(entity => new GuestContactResponse(entity));
}
