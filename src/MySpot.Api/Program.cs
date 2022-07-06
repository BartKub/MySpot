using MySpot.Application;
using MySpot.Core;
using MySpot.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddCore()
    .AddInfrastructure(builder.Configuration);

builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig.WriteTo.Console().WriteTo.File("logs.txt");
});

var app = builder.Build();
app.UseInfrastructure();

app.Run();
