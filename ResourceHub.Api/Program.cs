using Microsoft.EntityFrameworkCore;
using ResourceHub.Core.Interfaces;
using ResourceHub.Infrastructure.Persistence;
using ResourceHub.Infrastructure.Services;
using ResourceHub.Api.Mappings;
using ResourceHub.Api.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ResourceHubConnection")));

builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IResourceService, ResourceService>();

builder.Services.AddControllers();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Register all validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateResourceDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookingDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateResourceDtoValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<UpdateResourceDtoValidator>();

builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();


