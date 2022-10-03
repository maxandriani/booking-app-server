using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using BookingApp.Core.Guests.Queries;
using BookingApp.Core.Places.Models;
using BookingApp.Core.Places.ViewModels;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Places;

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