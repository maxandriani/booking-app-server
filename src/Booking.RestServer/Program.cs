using Booking.Core.Data;
using Booking.Core.Guests;
using Booking.Core.Guests.Models;
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
        typeof(UpdateGuestCmdHandler))
    .AddScoped<IValidator<Guest>, GuestValidator>();

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
