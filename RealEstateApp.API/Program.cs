using System.Text;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RealEstateApp.Application.Behaviors;
using RealEstateApp.Application.Interfaces;
using RealEstateApp.Infrastructure.Data;
using RealEstateApp.Infrastructure.Repositories;
using RealEstateApp.Infrastructure.Services;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

//---------- Database --------------------------------------------------------------------
builder.Services.AddDbContext<ApplicationDbContext>(options => 
options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("RealEstateApp.Infrastructure")
));

//---------- Repository Pattern & Unit of Work -------------------------------------------
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPropertyRepository, PropertyRepository>();
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));

//---------- JWT Service -----------------------------------------------------------------
builder.Services.AddScoped<IJwtService, JwtService>();

//---------- MediatR ---------------------------------------------------------------------
builder.Services.AddMediatR(cfg => 
    cfg.RegisterServicesFromAssembly(typeof(RealEstateApp.Application.Features.Users.Commands.RegisterUser.RegisterUserCommand).Assembly));

//---------- FluentValidation ------------------------------------------------------------
builder.Services.AddValidatorsFromAssembly(
    typeof(RealEstateApp.Application.Features.Users.Commands.RegisterUser.RegisterUserCommand).Assembly);
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

//---------- JWT Authentication ----------------------------------------------------------
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secretKey = Encoding.UTF8.GetBytes(jwtSettings["secretKey"]!);

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(secretKey)
    };
});

builder.Services.AddAuthorization();

//---------- Controllers -----------------------------------------------------------------
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();



//---------- Scalar (API Documentation) -------------------------------------------------
builder.Services.AddOpenApi();

//---------- Build App ------------------------------------------------------------------
var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();

