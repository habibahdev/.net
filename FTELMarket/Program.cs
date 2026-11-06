using FTELMarket.Infrastructure;
using FTELMarket.Infrastructure.Services;
using FTELMarket.Infrastructure.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// doc à l'@ : https://learn.microsoft.com/fr-fr/aspnet/core/security/cors?view=aspnetcore-10.0#np
builder.Services.AddCors(options =>
{
    options.AddPolicy("Front", policy => policy
    .WithOrigins("http://localhost:3000")
    .AllowAnyHeader()
    .AllowAnyMethod());
});

builder.Services.AddDbContext<FtelMarketDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("FtelMarket"))
);

builder.Services
.AddOptions<FlakySetting>()
.BindConfiguration(nameof(FlakySetting))
.ValidateDataAnnotations()
.ValidateOnStart();

// httpclient sera uniquement dispo dans le flakyservice + injection de dépenance
builder.Services.AddHttpClient<FlakyService>((sp, client) =>
{
    var settings = sp.GetRequiredService<IOptions<FlakySetting>>().Value;
    client.BaseAddress = new Uri(settings.BaseAddress);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseCors("Front");

// pas besoin de ça j'utilise pas http
//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
