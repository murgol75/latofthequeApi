using lutoftheque.api.Services;
using lutoftheque.bll.Services;
using lutoftheque.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// mise en place de la journalisation
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// configuration de Serilog pour la journalisation
Log.Logger = new LoggerConfiguration()
    //.WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Minute) // enregistre le log dans le dossier "logs"
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();


// Add services to the container.

// evite la recursivité lors des appels api avec Entity Framework
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
builder.Services.AddScoped<EventServiceBll>();
builder.Services.AddScoped<PlayerServiceBll>();
builder.Services.AddScoped<KeywordService>();
builder.Services.AddScoped<WeightCalculate>();



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
