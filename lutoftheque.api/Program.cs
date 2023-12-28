using lutoftheque.api.Services;
using lutoftheque.Entity.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var lutofthequeConnectionString = builder.Configuration.GetConnectionString("lutofthequeConnection");


builder.Services.AddDbContext<lutofthequeContext>(opts => opts.UseSqlServer(lutofthequeConnectionString));
builder.Services.AddTransient<GameService>();
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