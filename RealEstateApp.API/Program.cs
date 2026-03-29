using FluentValidation;
using MediatR;
using RealEstateApp.API.Middleware;
using RealEstateApp.Application.Behaviors;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Infrastructure.Data;
using Scalar.AspNetCore;
using Serilog;
using RealEstateApp.API.Extensions;
using Microsoft.EntityFrameworkCore;
using RealEstateApp.API.Hubs;

var builder = WebApplication.CreateBuilder(args);

//---------- Serilog ---------------------------------------------------------------------
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

//---------- Services --------------------------------------------------------------------

builder.Services.AddDatabase(builder.Configuration);
builder.Services.AddRepositories();
builder.Services.AddAuth(builder.Configuration);
builder.Services.AddCache(builder.Configuration);
builder.Services.AddCorsPolicy(builder.Configuration);
builder.Services.AddRateLimiting();
builder.Services.AddHealthChecksServices(builder.Configuration);

//---------- SignalR ---------------------------------------------------------------------
builder.Services.AddSignalR();

//---------- MediatR ---------------------------------------------------------------------
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(RealEstateApp.Application.Features.Users.Commands.RegisterUser.RegisterUserCommand).Assembly));

//---------- FluentValidation ------------------------------------------------------------
builder.Services.AddValidatorsFromAssembly(
    typeof(RealEstateApp.Application.Features.Users.Commands.RegisterUser.RegisterUserCommand).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));


//---------- API -----------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi();

//---------- Build App ------------------------------------------------------------------
var app = builder.Build();

//---------- Seed Database -------------------------------------------------------
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    await dbContext.Database.MigrateAsync();

    var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    await DatabaseSeeder.SeedAsync(unitOfWork, configuration);
}


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseSerilogRequestLogging();

app.UseCors("AllowFrontend");

app.UseRateLimiter();

app.UseMiddleware<GlobalExceptionHandlerMiddleware>();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapHub<NotificationHub>("/hubs/notifications");

app.UseHealthChecksEndpoint();

app.Run();

