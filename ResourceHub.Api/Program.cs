using Microsoft.EntityFrameworkCore;
using ResourceHub.Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ResourceHubConnection")));

var app = builder.Build();



app.Run();


