using MySpot.Application;
using MySpot.Core;
using MySpot.Infrastructure;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddCore()
    .AddInfrastructure(builder.Configuration);

var app = builder.Build();
app.UseInfrastructure();

app.Run();
