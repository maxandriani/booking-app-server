using Booking.Core.Commons.Handlers;
using Booking.Core.Data;
using Booking.Core.Places.Models;
using Booking.Core.Places.Queries;
using Booking.Core.Places.ViewModels;
using FluentValidation;

namespace Booking.Core.Places;

public class SearchPlaceQueryHandler :
    SearchEntityQueryHandlerBase<BookingDbContext, Place, SearchPlaceQuery, PlaceResponse>
{
    public SearchPlaceQueryHandler(
        BookingDbContext dbContext,
        IValidator<SearchPlaceQuery> validator) : base(dbContext, validator)
    {
    }

    protected override IQueryable<Place> ApplyDefaultSorting(IQueryable<Place> query)
        => query.OrderBy(q => q.Name);

    protected override IQueryable<Place> CreateSearchQuery(SearchPlaceQuery request)
    {
        var query = _dbContext.Places.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(q => q.Name.ToLower().StartsWith(request.Search.ToLower()));

        return query;
    }

    protected override PlaceResponse MapToResponse(Place entity)
        => new PlaceResponse(entity);
}