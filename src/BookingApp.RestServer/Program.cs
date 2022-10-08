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
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services
    .AddDbContextFactory<BookingDbContext>(options =>
    {
        options
            .UseNpgsql(builder.Configuration.GetConnectionString("BookingDb"), b => b.MigrationsAssembly("BookingApp.Core.Postgres"));
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
        typeof(CreateBookingCmdHandler),
        typeof(CancelBookingCmdHandler),
        typeof(AddBookingGuestCmdHandler),
        typeof(ConfirmBookingCmdHandler),

        typeof(GuestContactShallReferenceExistingGuest),
        typeof(PlaceNameShallBeUnique),
        typeof(BookingShallBeConfirmed))

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
    .AddScoped<SearchAvailablePlacesForBookingQueryHandler>()
    .AddScoped<PlaceNameShallBeUnique>()
    .AddScoped<CreateBookingCmdHandler>()
    .AddScoped<AddBookingGuestCmdHandler>()
    .AddScoped<CancelBookingCmdHandler>()
    .AddScoped<BookingShallBeConfirmed>()
    .AddScoped<ConfirmBookingCmdHandler>()
    .AddScoped<BookingShallNotOverlapSchedulesOnSamePlace>();

builder.Services.AddControllers();
builder.Services.AddApiVersioning(config =>
{
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
    config.ReportApiVersions = true;
    config.ApiVersionReader = new HeaderApiVersionReader("x-api-version");
});
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
