using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using BookingApp.Core.Places.Models;
using BookingApp.Core.Places.Queries;
using BookingApp.Core.Places.ViewModels;
using FluentValidation;

namespace BookingApp.Core.Places;

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

    protected override IQueryable<PlaceResponse> MapToResponse(IQueryable<Place> query)
        => query.Select(x => new PlaceResponse(x));
}