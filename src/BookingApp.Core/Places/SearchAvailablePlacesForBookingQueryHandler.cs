using BookingApp.Core.Places.Queries;
using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using BookingApp.Core.Places.Models;
using BookingApp.Core.Places.ViewModels;
using FluentValidation;

namespace BookingApp.Core.Places;

public class SearchAvailablePlacesForBookingQueryHandler :
    SearchEntityQueryHandlerBase<
        BookingDbContext,
        Place,
        SearchAvailablePlacesForBookingQuery,
        PlaceResponse>
{
    public SearchAvailablePlacesForBookingQueryHandler(
        BookingDbContext dbContext,
        IValidator<SearchAvailablePlacesForBookingQuery> validator) : base(dbContext, validator)
    {
    }

    protected override IQueryable<Place> ApplyDefaultSorting(IQueryable<Place> query)
        => query.OrderBy(q => q.Name);

    protected override IQueryable<Place> CreateSearchQuery(SearchAvailablePlacesForBookingQuery request)
    {
        var query = from place in _dbContext.Places
                    where !_dbContext.Bookings
                        .WhereOverlapBooking(request.CheckIn, request.CheckOut)
                        .Where(p => p.PlaceId == place.Id)
                        .Any()
                    select place;
        //select new BookingAvailabilityResponse(BookingAvailabilityStatusEnum.Available, new PlaceResponse(place));

        // For performance reasons I don't need booking info. Keeping this in case of rollback.
        // var query = from p in _dbContext.Places
        //             join b in _dbContext.Bookings.WhereOverlapBooking(request.CheckIn, request.CheckOut)
        //                 on p.Id equals b.PlaceId into grouping
        //             from b in grouping.DefaultIfEmpty()
        //             where b == null
        //             select p;

        return query;
    }

    protected override IQueryable<PlaceResponse> MapToResponse(IQueryable<Place> query)
        => query.Select(x => new PlaceResponse(x));

}