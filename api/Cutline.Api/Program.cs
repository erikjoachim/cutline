using Cutline.Api.Database;
using Cutline.Api.Features.Leagues;
using Cutline.Api.Features.Players;
using Cutline.Api.Features.Teams;
using Cutline.Api.Integrations.GolfApi;
using Cutline.Api.Jobs;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TickerQ.Dashboard.DependencyInjection;
using TickerQ.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddHttpClient<GolfApiClient>(o =>
{
    string baseUrl =
        builder.Configuration["GolfApi:BaseUrl"]
        ?? throw new NullReferenceException(
            "GolfApi:BaseUrl is null or not configured in appsettings"
        );
    o.BaseAddress = new Uri(baseUrl);
    o.DefaultRequestHeaders.Add("x-rapidapi-key", builder.Configuration["GolfApi:RapidApiKey"]);
    o.DefaultRequestHeaders.Add("x-rapidapi-host", builder.Configuration["GolfApi:RapidApiHost"]);
});

builder.Services.AddScoped<FetchPlayersJob>();
builder.Services.AddScoped<FetchTournamentsJob>();

builder.Services.AddOpenApi();

builder.Services.AddTickerQ(options =>
{
    options.AddDashboard(dashboardOptions =>
    {
        dashboardOptions.SetBasePath("/jobs");
        dashboardOptions.SetGroupName("ticker-jobs");
    });
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.MapPlayersEndpoints();

app.MapLeaguesEndpoints();

app.MapTeamsEndpoints();

app.UseTickerQ();

app.UseHttpsRedirection();

app.Run();
