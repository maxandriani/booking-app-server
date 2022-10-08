using BookingApp.Core.Bookings;
using BookingApp.Core.Bookings.Commands;
using BookingApp.Core.Bookings.Rules;
using BookingApp.Core.Bookings.Validations;
using BookingApp.Core.Data;
using BookingApp.Core.GuestContacts;
using BookingApp.Core.GuestContacts.Commands;
using BookingApp.Core.GuestContacts.Queries;
using BookingApp.Core.GuestContacts.Rules;
using BookingApp.Core.GuestContacts.Validations;
using BookingApp.Core.Guests;
using BookingApp.Core.Guests.Commands;
using BookingApp.Core.Guests.Queries;
using BookingApp.Core.Guests.Validations;
using BookingApp.Core.Places;
using BookingApp.Core.Places.Commands;
using BookingApp.Core.Places.Queries;
using BookingApp.Core.Places.Rules;
using BookingApp.Core.Places.Validations;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace BookingApp.Core.Tests.Commons;

public abstract class TestBase
{
    protected readonly IServiceProvider _rootInjector;
    protected readonly IServiceProvider _injector;
    protected readonly IMediator _mediator;

    public TestBase()
    {
        // var configuration = new ConfigurationBuilder().Build();
        var services = new ServiceCollection();

        services
            // .AddSingleton(configuration)
            .AddDbContextFactory<BookingDbContext>(options =>
            {
                options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString());
                options.EnableDetailedErrors(true);
                options.EnableSensitiveDataLogging(true);
                options.LogTo(Console.WriteLine, LogLevel.Information);
            })
            .AddLogging()
            .AddMediatR(
                typeof(CreateGuestCmdHandler),
                typeof(DeleteGuestCmdHandler),
                typeof(SearchGuestsQueryHandler),
                typeof(GetGuestByKeyQueryHandler),
                typeof(UpdateGuestCmdHandler),
                typeof(CreateGuestContactCmdHandler),
                typeof(DeleteGuestContactCmdHandler),
                typeof(SearchGuestContactQueryHandler),
                typeof(GetGuestContactByKeyQueryHandler),
                typeof(UpdateGuestContactCmdHandler),
                typeof(DeletePlaceCmdHandler),
                typeof(SearchPlaceQueryHandler),
                typeof(GetPlaceByKeyQueryHandler),
                typeof(UpdatePlaceCmdHandler),
                typeof(UpdatePlaceCmdHandler),
                typeof(AddBookingGuestCmdHandler),
                typeof(CreateBookingCmdHandler),
                typeof(ConfirmBookingCmdHandler),

                typeof(GuestContactShallReferenceExistingGuest),
                typeof(SearchAvailablePlacesForBookingQueryHandler))

            .AddScoped<IValidator<CreateGuestWithContactsCmd>, CreateGuestWithContactsCmdValidator>()
            .AddScoped<IValidator<DeleteGuestCmd>, DeleteGuestCmdValidator>()
            .AddScoped<IValidator<SearchGuestsQuery>, SearchGuestsQueryValidator>()
            .AddScoped<IValidator<GetGuestByKeyQuery>, GetGuestByKeyQueryValidator>()
            .AddScoped<IValidator<UpdateGuestCmd>, UpdateGuestCmdValidator>()
            .AddScoped<IValidator<CreateGuestContactCmd>, CreateGuestContactCmdValidator>()
            .AddScoped<IValidator<DeleteGuestContactCmd>, DeleteGuestContactCmdValidator>()
            .AddScoped<IValidator<SearchGuestContactQuery>, SearchGuestContactQueryValidator>()
            .AddScoped<IValidator<GetGuestContactByKeyQuery>, GetGuestContactByKeyQueryValidator>()
            .AddScoped<IValidator<UpdateGuestContactCmd>, UpdateGuestContactCmdValidator>()
            .AddScoped<IValidator<CreatePlaceCmd>, CreatePlaceCmdValidator>()
            .AddScoped<IValidator<DeletePlaceCmd>, DeletePlaceCmdValidator>()
            .AddScoped<IValidator<SearchPlaceQuery>, SearchPlaceQueryValidator>()
            .AddScoped<IValidator<GetPlaceByKeyQuery>, GetPlaceByKeyQueryValidator>()
            .AddScoped<IValidator<UpdatePlaceCmd>, UpdatePlaceCmdValidator>()
            .AddScoped<IValidator<SearchAvailablePlacesForBookingQuery>, SearchAvailablePlacesForBookingQueryValidator>()
            .AddScoped<IValidator<CreateBookingCmd>, CreateBookingCmdValidator>()
            .AddScoped<IValidator<AddBookingGuestCmd>, AddBookingGuestCmdValidator>()
            .AddScoped<IValidator<CancelBookingCmd>, CancelBookingCmdValidator>()
            .AddScoped<IValidator<ConfirmBookingCmd>, ConfirmBookingCmdValidator>()

            .AddScoped<CreateGuestCmdHandler>()
            .AddScoped<DeleteGuestCmdHandler>()
            .AddScoped<SearchGuestsQueryHandler>()
            .AddScoped<GetGuestByKeyQueryHandler>()
            .AddScoped<UpdateGuestCmdHandler>()
            .AddScoped<CreateGuestContactCmdHandler>()
            .AddScoped<DeleteGuestContactCmdHandler>()
            .AddScoped<SearchGuestContactQueryHandler>()
            .AddScoped<GetGuestContactByKeyQueryHandler>()
            .AddScoped<UpdateGuestContactCmdHandler>()
            .AddScoped<GuestContactShallReferenceExistingGuest>()
            .AddScoped<CreatePlaceCmdHandler>()
            .AddScoped<DeletePlaceCmdHandler>()
            .AddScoped<SearchPlaceQueryHandler>()
            .AddScoped<GetPlaceByKeyQueryHandler>()
            .AddScoped<UpdatePlaceCmdHandler>()
            .AddScoped<PlaceNameShallBeUnique>()
            .AddScoped<SearchAvailablePlacesForBookingQueryHandler>()
            .AddScoped<CreateBookingCmdHandler>()
            .AddScoped<AddBookingGuestCmdHandler>()
            .AddScoped<CancelBookingCmdHandler>()
            .AddScoped<BookingShallBeConfirmed>()
            .AddScoped<ConfirmBookingCmdHandler>()
            .AddScoped<BookingShallNotOverlapSchedulesOnSamePlace>();

        _rootInjector = services.BuildServiceProvider();
        _injector = _rootInjector.CreateScope().ServiceProvider;
        _mediator = _injector.GetRequiredService<IMediator>();
        _injector.GetRequiredService<BookingDbContext>().Database.EnsureDeleted();
    }
}