using Cutline.Api.Database;
using Cutline.Api.Integrations.GolfApi;
using Cutline.Api.Jobs;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TickerQ.Dashboard.DependencyInjection;
using TickerQ.DependencyInjection;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
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

builder.Services.AddOpenApi();

builder.Services.AddTickerQ(options =>
{
    options.AddDashboard(dashboardOptions =>
    {
        dashboardOptions.SetBasePath("/jobs");
    });
});

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseTickerQ();

app.UseHttpsRedirection();

app.Run();
