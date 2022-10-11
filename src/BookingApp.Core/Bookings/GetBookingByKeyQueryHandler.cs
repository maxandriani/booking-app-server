using BookingApp.Core.Bookings.Models;
using BookingApp.Core.Bookings.Queries;
using BookingApp.Core.Bookings.ViewModels;
using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Data;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Bookings;

public class GetBookingByKeyQueryHandler : GetEntityByKeyQueryHandlerBase<
    BookingDbContext,
    Booking,
    GetBookingByKeyQuery,
    BookingResponse>
{
    public GetBookingByKeyQueryHandler(
        BookingDbContext dbContext,
        IValidator<GetBookingByKeyQuery> validator) : base(dbContext, validator)
    {
    }

    protected override Task<Booking?> GetByKeyAsync(GetBookingByKeyQuery request)
        => _dbContext.Bookings
            .AsNoTracking()
            .Include(q => q.Place)
            .Include(q => q.Guests)
                .ThenInclude(q => q.Guest)
                    .ThenInclude(q => q!.Contacts)
            .FirstOrDefaultAsync(q => q.Id == request.Id);

    protected override BookingResponse MapToResponse(Booking entity)
        => new BookingResponse(entity);
}