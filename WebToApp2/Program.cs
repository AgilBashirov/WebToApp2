using Microsoft.EntityFrameworkCore;
using WebToApp2;
using WebToApp2.Helpers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
ConfigurationAccessor.AppConfiguration = builder.Configuration;
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: false)
    .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", optional: false, reloadOnChange: false);

// Database configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

// Register the DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsProduction())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/v1/swagger.json", " API"); });
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
