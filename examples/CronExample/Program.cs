using CronExample;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

// Same syntax as ASP.NET Core

var builder = Host.CreateApplicationBuilder(args);

// Take a look at ExampleScheduledService.cs
builder.Services.AddHostedService<ExampleScheduledService>();

var app = builder.Build();

app.Run();