using Microsoft.EntityFrameworkCore;
using ResourceHub.Core.Interfaces;
using ResourceHub.Infrastructure.Persistence;
using ResourceHub.Infrastructure.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ResourceHubConnection")));

builder.Services.AddScoped<IBookingService, BookingService>();

var app = builder.Build();



app.Run();


