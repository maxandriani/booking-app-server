using Booking.Core.Data;
using Booking.Core.GuestContacts;
using Booking.Core.GuestContacts.Commands;
using Booking.Core.GuestContacts.Events;
using Booking.Core.GuestContacts.Models;
using Booking.Core.GuestContacts.Queries;
using Booking.Core.GuestContacts.Validations;
using Booking.Core.Guests;
using Booking.Core.Guests.Commands;
using Booking.Core.Guests.Models;
using Booking.Core.Guests.Queries;
using Booking.Core.Guests.Validations;
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
        options.UseInMemoryDatabase(databaseName: "Booking.RestServer");
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
