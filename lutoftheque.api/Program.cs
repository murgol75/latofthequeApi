using lutoftheque.api.Services;
using lutoftheque.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// evite la recursivité lors des appels api
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
});

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var lutofthequeConnectionString = builder.Configuration.GetConnectionString("lutofthequeConnection");


builder.Services.AddDbContext<lutofthequeContext>(opts => opts.UseSqlServer(lutofthequeConnectionString));
builder.Services.AddTransient<PlayerService>();
builder.Services.AddTransient<GameService>();
builder.Services.AddTransient<EventService>();

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
