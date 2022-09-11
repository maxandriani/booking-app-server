using Booking.Core.Commons.Responses;
using Booking.Core.Data;
using Booking.Core.Guests.Queries;
using Booking.Core.Guests.Responses;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;

namespace Booking.Core.Guests;

public class SearchGuestsQueryHandler : IRequestHandler<SearchGuestsQuery, CollectionResponse<GuestResponse>>
{
    private readonly BookingDbContext _dbContext;

    public SearchGuestsQueryHandler(BookingDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<CollectionResponse<GuestResponse>> Handle(SearchGuestsQuery request, CancellationToken cancellationToken)
    {
        var query = _dbContext.Guests.AsQueryable();

        if (!string.IsNullOrWhiteSpace(request.Search))
            query = query.Where(q => q.Name.ToLower().StartsWith(request.Search.ToLower()));

        var count = await query.CountAsync(cancellationToken);

        if (request.Take != null)
            query = query.Take(request.Take.Value);

        if (request.Skip != null)
            query = query.Skip(request.Skip.Value);

        if (!string.IsNullOrWhiteSpace(request.Sort))
            query = query.OrderBy(request.Sort);

        var items = query.Select(x => new GuestResponse(x)).AsAsyncEnumerable();

        return new CollectionResponse<GuestResponse>(items, count, request.Take, request.Skip);
    }
}
