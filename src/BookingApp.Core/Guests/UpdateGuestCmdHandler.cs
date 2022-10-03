using BookingApp.Core.Commons.Exceptions;
using BookingApp.Core.Commons.Handlers;
using BookingApp.Core.Commons.Validators;
using BookingApp.Core.Data;
using BookingApp.Core.Guests.Commands;
using BookingApp.Core.Guests.Events;
using BookingApp.Core.Guests.Models;
using BookingApp.Core.Guests.ViewModels;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookingApp.Core.Guests;

public class UpdateGuestCmdHandler :
    UpdateCmdHandlerBase<BookingDbContext, Guest, UpdateGuestCmd, GuestWithContactsResponse, ValidateUpdateGuestCmdRules>
{
    public UpdateGuestCmdHandler(
        BookingDbContext dbContext,
        IValidator<UpdateGuestCmd> validator,
        IMediator mediator) : base(dbContext, validator, mediator)
    {
    }

    protected override Task<Guest?> GetByKeyAsync(UpdateGuestCmd request)
        => _dbContext.Guests
            .Include(q => q.Contacts)
            .FirstOrDefaultAsync(q => q.Id == request.Id);

    protected override ValidateUpdateGuestCmdRules MapToEvent(Guest entity)
        => new ValidateUpdateGuestCmdRules(entity);

    protected override GuestWithContactsResponse MapToResponse(Guest entity)
        => new GuestWithContactsResponse(entity);

    protected override void UpdateEntity(UpdateGuestCmd request, Guest entity)
    {
        var (_, name) = request;
        entity.Name = name;
    }
}