using FlightBoard.Application;
using FlightBoard.Application.Services;
using FlightBoard.Domain.Interfaces;
using FlightBoard.Infrastructure.Data;
using FlightBoard.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using FlightBoard.API.Hubs;
using FlightBoard.API.SignalR;
using FlightBoard.Application.Events;
using Microsoft.AspNetCore.Diagnostics;



var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContext<FlightDbContext>(options =>
    options.UseSqlite("Data Source=flights.db"));

builder.Services.AddScoped<IFlightRepository, FlightRepository>();
builder.Services.AddScoped<FlightService>();

builder.Services.AddSignalR();
builder.Services.AddScoped<IFlightEventPublisher, FlightEventPublisher>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin",
        builder =>
        {
            builder.WithOrigins("http://localhost:3000")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});


var app = builder.Build();

app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.ContentType = "application/json";

        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;
        if (exception is ArgumentException)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            await context.Response.WriteAsync($"Bad Request: {exception.Message}");
        }
        else
        {
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("An unexpected error occurred.");
        }
    });
});


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseCors("AllowSpecificOrigin");

app.MapControllers();

app.MapHub<FlightHub>("/flightHub");


app.Run();
