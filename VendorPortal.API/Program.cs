using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.OpenApi.Models;
using VendorPortal.API.Middleware;
using VendorPortal.Infrastructure.IoC;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);
builder.Services.AddControllers();

builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
    c.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.RouteValues["controller"]}_{apiDesc.RelativePath}");
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "VendorPortal.API", Version = "v1" });
});

builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddServices(builder.Configuration);

// builder.Services.AddHealthChecks()
//     .AddCheck<HealthCheckDisburseDataService>("hc", failureStatus: HealthStatus.Unhealthy);

builder.Services.Configure<HealthCheckPublisherOptions>(options =>
{
    options.Delay = TimeSpan.FromSeconds(2);
    options.Period = TimeSpan.FromSeconds(3600);
});

// builder.Services.AddSingleton<IHealthCheckPublisher, HealthCheckPublisher>();

// builder.Services.AddHostedService<RetryReceiverService>();
// builder.Services.AddHostedService<CreateTxnHistoryReceiverService>();
// builder.Services.AddHostedService<CreateTxnHistoryHoldReceiverService>();
builder.Services.AddResponseCompression();

var app = builder.Build();
app.UseSwagger();

app.UseResponseCompression();
// Configure the HTTP request pipeline.

app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "VendorPortal.API v1"));

app.UseRouting();
app.MapControllers();
// app.UseMiddleware<MiddlewareLogger>();

app.Run();