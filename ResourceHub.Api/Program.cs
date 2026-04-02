using Microsoft.EntityFrameworkCore;
using ResourceHub.Core.Interfaces;
using ResourceHub.Infrastructure.Persistence;
using ResourceHub.Infrastructure.Services;
using ResourceHub.Api.Mappings;
using ResourceHub.Api.Validation;
using FluentValidation;
using FluentValidation.AspNetCore;
using ResourceHub.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// DbContext configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ResourceHubConnection")));

// Dependency Injection for services
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddScoped<IResourceService, ResourceService>();

// Controllers 
builder.Services.AddControllers();

// Swagger configuration
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new()
    {
        Title = "ResourceHub API",
        Version = "v1",
        Description = "API for managing resources and bookings"
    });
});

// Fluent Validation configuration
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddFluentValidationClientsideAdapters();

// Register all validators
builder.Services.AddValidatorsFromAssemblyContaining<CreateBookingDtoValidator>();

// AutoMapper configuration
builder.Services.AddAutoMapper(typeof(MappingProfile));

var app = builder.Build();

// Global exception handling middleware
app.UseMiddleware<ExceptionMiddleware>();

// Swagger middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


