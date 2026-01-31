using Microsoft.EntityFrameworkCore;
using RealEstateApp.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// Database Configuration
builder.Services.AddDbContext<ApplicationDbContext>(options => 
options.UseSqlServer(
    builder.Configuration.GetConnectionString("DefaultConnection"),
    b => b.MigrationsAssembly("RealEstateApp.Infrastructure")
));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

