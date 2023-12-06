using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.RegularExpressions;
using WebWithMultipleSpa.Middlewares;
using Yarp.ReverseProxy.Transforms;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

var services = builder.Services;
//services.AddTelemetryConsumer<ForwarderTelemetry>();
//services.AddHttpContextAccessor();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast = Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast")
//.WithOpenApi()
;
app.MapReverseProxy();

//app.MapReverseProxy(proxyPipeline =>
//{
//    proxyPipeline.Use((context, next) =>
//    {
//        var endPoint = $"{context.Request.Scheme}://{context.Request.Host}";
//        var proxyFeature = context.GetReverseProxyFeature();
//        var routeId = proxyFeature.Route.Config.RouteId;

//        if (CheckIsSubpathRoute(context, routeId))
//        {
//            var routeValues = context.Request.RouteValues.Values;
//            var pathChilds = default(string);
//            foreach (var routeValue in routeValues)
//                pathChilds += $"/{routeValue}";
//            var redirectUri = $"{endPoint}/{routeId}{pathChilds}";
//            redirectUri += context.Request.QueryString.HasValue ? context.Request.QueryString.Value : null;

//            context.Response.Redirect(redirectUri);

//            return Task.CompletedTask;
//        }
//        return next();
//    });
//    proxyPipeline.UseSessionAffinity();
//    proxyPipeline.UseLoadBalancing();
//});

bool CheckIsSubpathRoute(HttpContext context, string routeId)
{
    var isMainPath = context.Request.Path.HasValue && Path.GetExtension(context.Request.Path.Value) == string.Empty;
    if (isMainPath)
    {
        return context.Request.Path.Value
            .Split("/")
            .Where(x => x != "")
            .ToArray()
            .Length > 1;
    }
    return isMainPath;
}

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}