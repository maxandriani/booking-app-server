using Booking.Core.Data;
using Booking.Core.GuestContacts;
using Booking.Core.GuestContacts.Commands;
using Booking.Core.GuestContacts.Events;
using Booking.Core.GuestContacts.Models;
using Booking.Core.GuestContacts.Queries;
using Booking.Core.GuestContacts.Validations;
using Booking.Core.Guests;
using Booking.Core.Guests.Commands;
using Booking.Core.Guests.Events;
using Booking.Core.Guests.Models;
using Booking.Core.Guests.Queries;
using Booking.Core.Guests.Validations;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Booking.Core.Tests.Commons;

public abstract class BaseTest
{
    protected readonly IServiceProvider _rootInjector;
    protected readonly IServiceProvider _injector;

    public BaseTest()
    {
        // var configuration = new ConfigurationBuilder().Build();
        var services = new ServiceCollection();

        services
            // .AddSingleton(configuration)
            .AddDbContextFactory<BookingDbContext>(options => options.UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()))
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

                typeof(GuestContactShallReferenceExistingGuest))

            .AddScoped<IValidator<Guest>, GuestValidator>()
            .AddScoped<IValidator<GuestContact>, GuestContactValidator>()

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
            .AddScoped<GuestContactShallReferenceExistingGuest>();

        _rootInjector = services.BuildServiceProvider();
        _injector = _rootInjector.CreateScope().ServiceProvider;
    }
}