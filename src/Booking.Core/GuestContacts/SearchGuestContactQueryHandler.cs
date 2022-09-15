using Booking.Core.Commons.Handlers;
using Booking.Core.Data;
using Booking.Core.GuestContacts.Models;
using Booking.Core.GuestContacts.Queries;
using Booking.Core.GuestContacts.ViewModels;
using FluentValidation;

namespace Booking.Core.GuestContacts;

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

    protected override GuestContactResponse MapToResponse(GuestContact entity)
        => new GuestContactResponse(entity);
}