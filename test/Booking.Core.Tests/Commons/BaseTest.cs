using Booking.Core.Data;
using Booking.Core.GuestContacts;
using Booking.Core.GuestContacts.Commands;
using Booking.Core.GuestContacts.Queries;
using Booking.Core.GuestContacts.Rules;
using Booking.Core.GuestContacts.Validations;
using Booking.Core.Guests;
using Booking.Core.Guests.Commands;
using Booking.Core.Guests.Queries;
using Booking.Core.Guests.Validations;
using Booking.Core.Places;
using Booking.Core.Places.Commands;
using Booking.Core.Places.Queries;
using Booking.Core.Places.Rules;
using Booking.Core.Places.Validations;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Booking.Core.Tests.Commons;

public abstract class BaseTest
{
    protected readonly IServiceProvider _rootInjector;
    protected readonly IServiceProvider _injector;
    protected readonly IMediator _mediator;

    public BaseTest()
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
            .AddScoped<SearchAvailablePlacesForBookingQueryHandler>();

        _rootInjector = services.BuildServiceProvider();
        _injector = _rootInjector.CreateScope().ServiceProvider;
        _mediator = _injector.GetRequiredService<IMediator>();
    }
}