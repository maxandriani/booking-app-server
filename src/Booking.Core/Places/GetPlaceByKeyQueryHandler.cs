using Booking.Core.Commons.Handlers;
using Booking.Core.Data;
using Booking.Core.Guests.Queries;
using Booking.Core.Places.Models;
using Booking.Core.Places.ViewModels;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Booking.Core.Places;

public class GetPlaceByKeyQueryHandler :
    GetEntityByKeyQueryHandlerBase<BookingDbContext, Place, GetPlaceByKeyQuery, PlaceResponse>
{
    public GetPlaceByKeyQueryHandler(
        BookingDbContext dbContext,
        IValidator<GetPlaceByKeyQuery> validator) : base(dbContext, validator)
    {
    }

    protected override Task<Place?> GetByKeyAsync(GetPlaceByKeyQuery request)
        => _dbContext.Places.FirstOrDefaultAsync(q => q.Id == request.Id);

    protected override PlaceResponse MapToResponse(Place entity)
        => new PlaceResponse(entity);
}