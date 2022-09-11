using Booking.Core.Data;
using Booking.Core.Guests;
using Booking.Core.Guests.Models;
using Booking.Core.Guests.Validations;
using FluentValidation;
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
            .AddTransient<CreateGuestCmdHandler>()
            .AddTransient<DeleteGuestCmdHandler>()
            .AddTransient<SearchGuestsQueryHandler>()
            .AddTransient<GetGuestByKeyQueryHandler>()
            .AddTransient<UpdateGuestCmdHandler>()
            .AddScoped<IValidator<Guest>, GuestValidator>();

        _rootInjector = services.BuildServiceProvider();
        _injector = _rootInjector.CreateScope().ServiceProvider;
    }
}