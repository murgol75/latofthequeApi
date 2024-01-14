using lutoftheque.api.Services;
using lutoftheque.bll.Services;
using lutoftheque.Entity.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Serilog;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using lutoftheque.api.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Ajoutez cette configuration pour CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("MyAllowSpecificOrigins",
    builder =>
    {
        builder.WithOrigins("http://localhost:4200")
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});


// recuperer les infos de config de jwt à partir du fichier appsettings.json et le stocker dans la classe prévue
JwtOptions options = builder.Configuration.GetSection("JwtOptions").Get<JwtOptions>();

// injection de jwtOptions
builder.Services.AddSingleton(options);

// configuration de l'auth dans les services
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer
    (
    o =>
    {
        // recuperer la clé de signature
        byte[] sKey = Encoding.UTF8.GetBytes(options.SigningKey);
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = options.Issuer,
            ValidAudience = options.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(sKey)
        };
    }
    );
builder.Services.AddAuthorization();

// mise en place de la journalisation
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddDebug();

// configuration de Serilog pour la journalisation
Log.Logger = new LoggerConfiguration()
    .WriteTo.File("logs/myapp.txt", rollingInterval: RollingInterval.Minute) // enregistre le log dans le dossier "logs"
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();


// Add services to the container.

// evite la recursivité lors des appels api avec Entity Framework
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
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
builder.Services.AddScoped<GameServiceBll>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<AuthServiceBll>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else 
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}



app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("MyAllowSpecificOrigins");

app.MapControllers();

app.Run();
